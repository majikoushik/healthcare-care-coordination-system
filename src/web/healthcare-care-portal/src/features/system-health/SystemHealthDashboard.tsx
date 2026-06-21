import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useSecurity } from '../../core/security/SecurityContext';

interface HealthCheckEntry {
  name: string;
  status: string;
  description?: string;
  duration: string;
}

interface ServiceHealth {
  service: string;
  status: string;
  environment: string;
  correlationId: string;
  timestamp: string;
  totalDuration: string;
  checks: HealthCheckEntry[];
  _error?: string;
}

const endpoints = [
  { name: 'Patient.Api', url: 'http://localhost:5080/health/ready' },
  { name: 'Provider.Api', url: 'http://localhost:5081/health/ready' },
  { name: 'Appointment.Api', url: 'http://localhost:5082/health/ready' },
  { name: 'CarePlan.Api', url: 'http://localhost:5083/health/ready' },
  { name: 'ClinicalInsights.Api', url: 'http://localhost:5084/health/ready' },
  { name: 'Audit.Api', url: 'http://localhost:5085/health/ready' }
  // Notification Worker omitted from direct UI polling as it might not be exposed
];

export const SystemHealthDashboard: React.FC = () => {
  const [healthData, setHealthData] = useState<Record<string, ServiceHealth>>({});
  const [loading, setLoading] = useState(true);
  const { hasPermission } = useSecurity();

  const fetchHealth = async () => {
    setLoading(true);
    const results: Record<string, ServiceHealth> = {};
    
    for (const ep of endpoints) {
      try {
        const response = await axios.get(ep.url);
        results[ep.name] = response.data;
      } catch (err: any) {
        results[ep.name] = {
          service: ep.name,
          status: 'Unhealthy',
          environment: 'Unknown',
          correlationId: err.response?.headers?.['x-correlation-id'] || 'N/A',
          timestamp: new Date().toISOString(),
          totalDuration: '0',
          checks: [],
          _error: err.message
        };
      }
    }
    
    setHealthData(results);
    setLoading(false);
  };

  useEffect(() => {
    fetchHealth();
  }, []);

  if (!hasPermission("SystemHealth.Read")) {
    return (
      <div className="page-section">
        <h2 style={{ color: 'red' }}>Access Denied</h2>
        <p>You do not have permission to view System Health metrics.</p>
      </div>
    );
  }

  const getStatusColor = (status: string) => {
    if (status === 'Healthy') return '#167455';
    if (status === 'Degraded') return '#b7791f';
    return '#c53030';
  };

  return (
    <div className="page-section">
      <div className="section-header" style={{ marginBottom: '20px', display: 'flex', justifyContent: 'space-between' }}>
        <h2>System Health Dashboard</h2>
        <button 
          onClick={fetchHealth} 
          disabled={loading}
          style={{ background: '#1a365d', color: 'white', border: 'none', padding: '8px 16px', borderRadius: '4px', cursor: 'pointer', fontWeight: 600 }}
        >
          {loading ? 'Refreshing...' : 'Refresh Status'}
        </button>
      </div>

      <div className="dashboard-intro" style={{ marginBottom: '24px' }}>
        <p>This dashboard pulls live operational telemetry from the `/health/ready` endpoints across all backend microservices. OpenTelemetry and Application Insights are configured in the backend for deeper tracing in Azure Monitor.</p>
      </div>

      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))', gap: '20px' }}>
        {endpoints.map(ep => {
          const data = healthData[ep.name];
          if (!data) return <div key={ep.name} className="info-panel">Loading {ep.name}...</div>;

          return (
            <div key={ep.name} className="info-panel" style={{ borderTop: `4px solid ${getStatusColor(data.status)}` }}>
              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: '16px' }}>
                <h3 style={{ margin: 0 }}>{data.service}</h3>
                <span className="badge" style={{ background: data.status === 'Healthy' ? '#e8f7f0' : '#fce8e8', color: getStatusColor(data.status) }}>
                  {data.status}
                </span>
              </div>
              
              <div style={{ fontSize: '0.85rem', color: '#4a5568', marginBottom: '16px' }}>
                <div><strong>Env:</strong> {data.environment}</div>
                <div><strong>Latency:</strong> {data.totalDuration}</div>
                <div><strong>Correlation ID:</strong> <span style={{ fontFamily: 'monospace' }}>{data.correlationId}</span></div>
                <div><strong>Last Checked:</strong> {new Date(data.timestamp).toLocaleTimeString()}</div>
              </div>

              {data._error && (
                <div style={{ color: '#c53030', fontSize: '0.85rem', marginBottom: '12px' }}>
                  <strong>Error:</strong> {data._error}
                </div>
              )}

              {data.checks && data.checks.length > 0 && (
                <div style={{ borderTop: '1px solid #e2e8f0', paddingTop: '12px' }}>
                  <h4 style={{ fontSize: '0.85rem', margin: '0 0 8px 0', color: '#2d3748' }}>Dependencies:</h4>
                  <ul style={{ listStyle: 'none', padding: 0, margin: 0, fontSize: '0.85rem' }}>
                    {data.checks.map(c => (
                      <li key={c.name} style={{ display: 'flex', justifyContent: 'space-between', padding: '4px 0' }}>
                        <span>{c.name}</span>
                        <span style={{ color: getStatusColor(c.status), fontWeight: 500 }}>{c.status}</span>
                      </li>
                    ))}
                  </ul>
                </div>
              )}
            </div>
          );
        })}
      </div>
    </div>
  );
};

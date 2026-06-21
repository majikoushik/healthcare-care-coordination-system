import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { carePlanApi } from '../../core/api/carePlanApi';
import { CarePlanDocument, CarePlanStatus } from './types';
import { useSecurity } from '../../core/security/SecurityContext';
import { PrivacyNotice } from '../../shared/ui/PrivacyNotice';

export const CarePlanList: React.FC = () => {
  const [plans, setPlans] = useState<CarePlanDocument[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const { hasPermission } = useSecurity();

  useEffect(() => {
    const fetchPlans = async () => {
      try {
        const data = await carePlanApi.getCarePlans();
        setPlans(data);
      } catch (err) {
        setError('Failed to load care plans.');
      } finally {
        setLoading(false);
      }
    };
    fetchPlans();
  }, []);

  const getStatusBadge = (status: CarePlanStatus) => {
    switch (status) {
      case CarePlanStatus.Draft:
        return <span className="badge" style={{ background: '#e2e8f0', color: '#4a5568' }}>Draft</span>;
      case CarePlanStatus.Active:
        return <span className="badge" style={{ background: '#e8f7f0', color: '#167455' }}>Active</span>;
      case CarePlanStatus.OnHold:
        return <span className="badge" style={{ background: '#fefcbf', color: '#744210' }}>On Hold</span>;
      case CarePlanStatus.Completed:
        return <span className="badge" style={{ background: '#c6f6d5', color: '#22543d' }}>Completed</span>;
      case CarePlanStatus.Cancelled:
        return <span className="badge" style={{ background: '#fed7d7', color: '#822727' }}>Cancelled</span>;
      default:
        return <span className="badge">Unknown</span>;
    }
  };

  return (
    <div className="page-section">
      <PrivacyNotice />
      <div className="section-header">
        <h2>Care Plans</h2>
        {hasPermission("CarePlan.Write") && (
          <Link to="/care-plans/new" style={{ background: '#21a67a', color: 'white', padding: '8px 16px', borderRadius: '4px', textDecoration: 'none', fontWeight: 600 }}>
            + New Plan
          </Link>
        )}
      </div>

      <div className="dashboard-intro" style={{ marginTop: '20px', display: 'block' }}>
        {loading ? (
          <p>Loading care plans...</p>
        ) : error ? (
          <p style={{ color: 'red' }}>{error}</p>
        ) : plans.length === 0 ? (
          <p>No care plans found.</p>
        ) : (
          <div style={{ overflowX: 'auto' }}>
            <table style={{ width: '100%', borderCollapse: 'collapse', textAlign: 'left' }}>
              <thead>
                <tr style={{ borderBottom: '2px solid #d9e4e7' }}>
                  <th style={{ padding: '12px 8px' }}>Title</th>
                  <th style={{ padding: '12px 8px' }}>Created</th>
                  <th style={{ padding: '12px 8px' }}>Follow Up</th>
                  <th style={{ padding: '12px 8px' }}>Tasks</th>
                  <th style={{ padding: '12px 8px' }}>Status</th>
                  <th style={{ padding: '12px 8px' }}>Action</th>
                </tr>
              </thead>
              <tbody>
                {plans.map(p => (
                  <tr key={p.id} style={{ borderBottom: '1px solid #d9e4e7' }}>
                    <td style={{ padding: '12px 8px', fontWeight: 600 }}>{p.title}</td>
                    <td style={{ padding: '12px 8px' }}>{new Date(p.createdAt).toLocaleDateString()}</td>
                    <td style={{ padding: '12px 8px' }}>{p.followUpDate ? new Date(p.followUpDate).toLocaleDateString() : 'N/A'}</td>
                    <td style={{ padding: '12px 8px' }}>{p.tasks?.length || 0}</td>
                    <td style={{ padding: '12px 8px' }}>{getStatusBadge(p.status)}</td>
                    <td style={{ padding: '12px 8px' }}>
                      <Link to={`/care-plans/${p.id}`} style={{ color: '#21a67a', textDecoration: 'none', fontWeight: 600 }}>View</Link>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
};

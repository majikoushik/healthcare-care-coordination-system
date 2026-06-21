import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { notificationApi } from '../../core/api/notificationApi';
import { NotificationRecord, NotificationStatus } from './types';

export const NotificationDetails: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [record, setRecord] = useState<NotificationRecord | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [isSimulating, setIsSimulating] = useState(false);
  const [simulationResult, setSimulationResult] = useState<{ success: boolean; message: string } | null>(null);

  useEffect(() => {
    if (id) {
      fetchRecord(id);
    }
  }, [id]);

  const fetchRecord = async (recordId: string) => {
    try {
      setLoading(true);
      const data = await notificationApi.getNotificationById(recordId);
      setRecord(data);
    } catch (err) {
      setError('Failed to fetch notification details.');
    } finally {
      setLoading(false);
    }
  };

  const handleSimulateSend = async () => {
    if (!record) return;
    try {
      setIsSimulating(true);
      setSimulationResult(null);
      const response = await notificationApi.simulateSend(record.id);
      
      setSimulationResult({ success: true, message: response.message });
      // Refresh to get updated status/attempt count
      await fetchRecord(record.id);
    } catch (err: any) {
      setSimulationResult({ 
        success: false, 
        message: err.response?.data?.detail || 'Failed to simulate sending notification.' 
      });
    } finally {
      setIsSimulating(false);
    }
  };

  if (loading) return <div>Loading notification details...</div>;
  if (!record) return <div>Notification not found</div>;

  const canSimulate = record.status === NotificationStatus.Requested || record.status === NotificationStatus.Queued || record.status === NotificationStatus.SimulatedFailed;

  return (
    <div style={{ padding: '24px', maxWidth: '800px', margin: '0 auto' }}>
      <button onClick={() => navigate('/notifications')} style={{ marginBottom: '16px', background: 'none', border: 'none', color: '#3182ce', cursor: 'pointer', padding: 0 }}>
        &larr; Back to Event Stream
      </button>

      {error && <div style={{ color: 'red', marginBottom: '16px', padding: '12px', background: '#ffe6e6', borderRadius: '4px' }}>{error}</div>}

      <div style={{ background: '#eebf2422', border: '1px solid #eebf24', padding: '16px', borderRadius: '8px', marginBottom: '24px' }}>
        <strong>Privacy Notice:</strong> This payload demonstrates event persistence and timeline history. Full raw clinical notes are never stored in the notification document.
      </div>

      <div style={{ background: 'white', padding: '24px', borderRadius: '8px', boxShadow: '0 1px 3px rgba(0,0,0,0.1)' }}>
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: '24px' }}>
          <div>
            <h1 style={{ fontSize: '24px', fontWeight: 'bold', marginBottom: '8px' }}>{record.subject}</h1>
            <p style={{ color: '#718096' }}>ID: {record.id}</p>
          </div>
          <span style={{ background: '#edf2f7', padding: '8px 16px', borderRadius: '4px', fontWeight: 'bold' }}>
            Status: {NotificationStatus[record.status]}
          </span>
        </div>
        
        <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px', marginBottom: '24px' }}>
          <div><strong>Type:</strong> {record.notificationType}</div>
          <div><strong>Recipient:</strong> {record.recipientReference}</div>
          <div><strong>Entity Type:</strong> {record.relatedEntityType}</div>
          <div><strong>Entity ID:</strong> {record.relatedEntityId}</div>
          <div><strong>Created:</strong> {new Date(record.createdAt).toLocaleString()}</div>
          <div><strong>Sent At:</strong> {record.sentAt ? new Date(record.sentAt).toLocaleString() : 'N/A'}</div>
          <div><strong>Attempts:</strong> {record.attemptCount}</div>
          {record.patientId && <div><strong>Patient ID:</strong> <span style={{ cursor: 'pointer', color: '#3182ce' }} onClick={() => navigate(`/patients/${record.patientId}`)}>{record.patientId}</span></div>}
        </div>

        <div style={{ marginBottom: '24px' }}>
          <strong>Message Summary:</strong>
          <p style={{ marginTop: '8px', background: '#f7fafc', padding: '16px', borderRadius: '4px', fontStyle: 'italic' }}>"{record.messageSummary}"</p>
        </div>

        <div style={{ borderTop: '1px solid #e2e8f0', paddingTop: '24px' }}>
          <h3 style={{ fontWeight: 'bold', marginBottom: '16px' }}>Service Bus Simulation</h3>
          <p style={{ marginBottom: '16px', color: '#4a5568' }}>Clicking "Simulate Dispatch" mimics a background worker popping this message off the queue and attempting delivery. It increments the attempt counter and sets the Sent timestamp.</p>
          
          <div style={{ display: 'flex', gap: '16px', alignItems: 'center' }}>
            <button 
              onClick={handleSimulateSend}
              disabled={isSimulating || !canSimulate}
              style={{ background: canSimulate ? '#3182ce' : '#cbd5e0', color: 'white', padding: '8px 16px', borderRadius: '4px', border: 'none', cursor: canSimulate ? 'pointer' : 'not-allowed' }}
            >
              {isSimulating ? 'Simulating...' : 'Simulate Dispatch'}
            </button>
          </div>

          {simulationResult && (
            <div style={{ marginTop: '16px', padding: '12px', background: simulationResult.success ? '#f0fff4' : '#fff5f5', color: simulationResult.success ? '#2f855a' : '#c53030', borderRadius: '4px' }}>
              {simulationResult.message}
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

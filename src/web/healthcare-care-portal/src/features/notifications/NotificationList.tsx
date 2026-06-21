import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { notificationApi } from '../../core/api/notificationApi';
import { NotificationRecord, NotificationStatus, NotificationChannel } from './types';

export const NotificationList: React.FC = () => {
  const [notifications, setNotifications] = useState<NotificationRecord[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    fetchNotifications();
  }, []);

  const fetchNotifications = async () => {
    try {
      setLoading(true);
      const data = await notificationApi.getNotifications();
      setNotifications(data);
    } catch (err) {
      setError('Failed to fetch simulated notifications.');
    } finally {
      setLoading(false);
    }
  };

  const getStatusBadge = (status: NotificationStatus) => {
    switch (status) {
      case NotificationStatus.Requested: return <span style={{ background: '#edf2f7', padding: '4px 8px', borderRadius: '4px' }}>Requested</span>;
      case NotificationStatus.Queued: return <span style={{ background: '#feebc8', color: '#dd6b20', padding: '4px 8px', borderRadius: '4px' }}>Queued</span>;
      case NotificationStatus.SimulatedSent: return <span style={{ background: '#f0fff4', color: '#38a169', padding: '4px 8px', borderRadius: '4px' }}>Simulated Sent</span>;
      case NotificationStatus.SimulatedFailed: return <span style={{ background: '#fff5f5', color: '#e53e3e', padding: '4px 8px', borderRadius: '4px' }}>Simulated Failed</span>;
      case NotificationStatus.Cancelled: return <span style={{ background: '#e2e8f0', color: '#718096', padding: '4px 8px', borderRadius: '4px' }}>Cancelled</span>;
      default: return null;
    }
  };

  const getChannelBadge = (channel: NotificationChannel) => {
    switch (channel) {
      case NotificationChannel.EmailSimulation: return <span style={{ background: '#ebf8ff', color: '#3182ce', padding: '4px 8px', borderRadius: '4px', fontSize: '12px' }}>Email</span>;
      case NotificationChannel.SmsSimulation: return <span style={{ background: '#f0fff4', color: '#38a169', padding: '4px 8px', borderRadius: '4px', fontSize: '12px' }}>SMS</span>;
      case NotificationChannel.PortalSimulation: return <span style={{ background: '#faf5ff', color: '#805ad5', padding: '4px 8px', borderRadius: '4px', fontSize: '12px' }}>Portal</span>;
      default: return null;
    }
  };

  if (loading) return <div>Loading notifications...</div>;

  return (
    <div style={{ padding: '24px' }}>
      <div style={{ background: '#eebf2422', border: '1px solid #eebf24', padding: '16px', borderRadius: '8px', marginBottom: '24px' }}>
        <strong>Privacy Notice:</strong> All notifications in this portal are <strong>simulated</strong>. No real external emails, SMS, or push notifications are dispatched from this platform. This is to ensure compliance with strict healthcare data safety rules.
      </div>

      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '24px' }}>
        <h1 style={{ fontSize: '24px', fontWeight: 'bold' }}>Simulated Notifications</h1>
        <button 
          onClick={() => navigate('/notifications/new')}
          style={{ background: '#3182ce', color: 'white', padding: '8px 16px', borderRadius: '4px', border: 'none', cursor: 'pointer' }}
        >
          Simulate Notification
        </button>
      </div>

      {error && <div style={{ color: 'red', marginBottom: '16px' }}>{error}</div>}

      {notifications.length === 0 ? (
        <div style={{ background: '#f7fafc', padding: '32px', textAlign: 'center', borderRadius: '8px' }}>
          <p>No notifications found in the event stream.</p>
        </div>
      ) : (
        <div style={{ display: 'grid', gap: '16px' }}>
          {notifications.map(record => (
            <div 
              key={record.id} 
              onClick={() => navigate(`/notifications/${record.id}`)}
              style={{ border: '1px solid #e2e8f0', padding: '16px', borderRadius: '8px', cursor: 'pointer', background: 'white', boxShadow: '0 1px 3px rgba(0,0,0,0.1)' }}
            >
              <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '8px' }}>
                <h3 style={{ fontWeight: 'bold', fontSize: '16px' }}>{record.subject}</h3>
                <div style={{ display: 'flex', gap: '8px' }}>
                  {getChannelBadge(record.channel)}
                  {getStatusBadge(record.status)}
                </div>
              </div>
              <p style={{ color: '#4a5568', fontSize: '14px', marginBottom: '16px' }}>{record.notificationType} &bull; {record.recipientReference}</p>
              
              <div style={{ display: 'flex', gap: '16px', fontSize: '12px', color: '#718096' }}>
                <span><strong>Created:</strong> {new Date(record.createdAt).toLocaleString()}</span>
                {record.sentAt && <span><strong>Sent:</strong> {new Date(record.sentAt).toLocaleString()}</span>}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

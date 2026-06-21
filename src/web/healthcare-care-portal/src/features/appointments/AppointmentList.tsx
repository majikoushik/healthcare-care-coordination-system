import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { appointmentApi } from '../../core/api/appointmentApi';
import { Appointment, AppointmentType, AppointmentStatus } from './types';

export const AppointmentList: React.FC = () => {
  const [appointments, setAppointments] = useState<Appointment[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchAppointments = async () => {
      try {
        const data = await appointmentApi.getAppointments();
        setAppointments(data);
      } catch (err) {
        setError('Failed to load appointments.');
      } finally {
        setLoading(false);
      }
    };
    fetchAppointments();
  }, []);

  const getTypeString = (t: AppointmentType) => {
    switch (t) {
      case AppointmentType.Consultation: return 'Consultation';
      case AppointmentType.FollowUp: return 'Follow Up';
      case AppointmentType.LabReview: return 'Lab Review';
      case AppointmentType.MedicationReview: return 'Medication Review';
      case AppointmentType.CarePlanReview: return 'Care Plan Review';
      default: return 'Unknown';
    }
  };

  const getStatusBadge = (status: AppointmentStatus) => {
    switch (status) {
      case AppointmentStatus.Requested:
        return <span className="badge" style={{ background: '#ebf8ff', color: '#2b6cb0' }}>Requested</span>;
      case AppointmentStatus.Scheduled:
        return <span className="badge" style={{ background: '#e8f7f0', color: '#167455' }}>Scheduled</span>;
      case AppointmentStatus.CheckedIn:
        return <span className="badge" style={{ background: '#faf089', color: '#744210' }}>Checked In</span>;
      case AppointmentStatus.Completed:
        return <span className="badge" style={{ background: '#c6f6d5', color: '#22543d' }}>Completed</span>;
      case AppointmentStatus.Cancelled:
        return <span className="badge" style={{ background: '#fed7d7', color: '#822727' }}>Cancelled</span>;
      case AppointmentStatus.NoShow:
        return <span className="badge" style={{ background: '#e2e8f0', color: '#4a5568' }}>No Show</span>;
      default:
        return <span className="badge">Unknown</span>;
    }
  };

  return (
    <div className="page-section">
      <div className="section-header">
        <h2>Appointments</h2>
        <Link to="/appointments/new" style={{ background: '#21a67a', color: 'white', padding: '8px 16px', borderRadius: '4px', textDecoration: 'none', fontWeight: 600 }}>
          + Schedule
        </Link>
      </div>

      <div className="dashboard-intro" style={{ marginTop: '20px', display: 'block' }}>
        {loading ? (
          <p>Loading appointments...</p>
        ) : error ? (
          <p style={{ color: 'red' }}>{error}</p>
        ) : appointments.length === 0 ? (
          <p>No appointments found.</p>
        ) : (
          <div style={{ overflowX: 'auto' }}>
            <table style={{ width: '100%', borderCollapse: 'collapse', textAlign: 'left' }}>
              <thead>
                <tr style={{ borderBottom: '2px solid #d9e4e7' }}>
                  <th style={{ padding: '12px 8px' }}>Date & Time</th>
                  <th style={{ padding: '12px 8px' }}>Type</th>
                  <th style={{ padding: '12px 8px' }}>Reason</th>
                  <th style={{ padding: '12px 8px' }}>Status</th>
                  <th style={{ padding: '12px 8px' }}>Action</th>
                </tr>
              </thead>
              <tbody>
                {appointments.map(a => (
                  <tr key={a.id} style={{ borderBottom: '1px solid #d9e4e7' }}>
                    <td style={{ padding: '12px 8px', fontWeight: 600 }}>{new Date(a.appointmentDateTime).toLocaleString()}</td>
                    <td style={{ padding: '12px 8px' }}>{getTypeString(a.type)}</td>
                    <td style={{ padding: '12px 8px' }}>{a.visitReason}</td>
                    <td style={{ padding: '12px 8px' }}>{getStatusBadge(a.status)}</td>
                    <td style={{ padding: '12px 8px' }}>
                      <Link to={`/appointments/${a.id}`} style={{ color: '#21a67a', textDecoration: 'none', fontWeight: 600 }}>View</Link>
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

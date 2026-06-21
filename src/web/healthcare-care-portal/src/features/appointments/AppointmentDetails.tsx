import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { appointmentApi } from '../../core/api/appointmentApi';
import { Appointment, AppointmentType, AppointmentStatus } from './types';

export const AppointmentDetails: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [appointment, setAppointment] = useState<Appointment | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [updating, setUpdating] = useState(false);

  const fetchAppointment = async () => {
    if (!id) return;
    try {
      setLoading(true);
      const data = await appointmentApi.getAppointmentById(id);
      setAppointment(data);
    } catch (err) {
      setError('Failed to load appointment details.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchAppointment();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [id]);

  const handleStatusUpdate = async (newStatus: AppointmentStatus) => {
    if (!id || !appointment) return;
    setUpdating(true);
    setError(null);
    try {
      await appointmentApi.updateAppointmentStatus(id, {
        status: newStatus,
        reason: 'Updated via Portal',
        updatedBy: 'SystemUser' // Would be actual logged in user in future
      });
      await fetchAppointment();
    } catch (err: any) {
      setError(err.response?.data?.detail || 'Failed to update status due to validation rules.');
    } finally {
      setUpdating(false);
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

  if (loading) return <div className="page-section"><p>Loading appointment...</p></div>;
  if (!appointment) return <div className="page-section"><p style={{ color: 'red' }}>Appointment not found.</p></div>;

  return (
    <div className="page-section">
      <div className="section-header" style={{ marginBottom: '20px' }}>
        <div style={{ display: 'flex', alignItems: 'center', gap: '16px' }}>
          <Link to="/appointments" style={{ textDecoration: 'none', color: '#62707d', fontWeight: 600 }}>&larr; Back</Link>
          <h2 style={{ margin: 0 }}>Appointment Details</h2>
          {getStatusBadge(appointment.status)}
        </div>
      </div>

      {error && <div style={{ color: 'red', marginBottom: '16px', padding: '12px', background: '#ffe6e6', borderRadius: '4px' }}>{error}</div>}

      <div className="module-grid" style={{ gridTemplateColumns: '2fr 1fr' }}>
        <div className="info-panel">
          <h3>Information</h3>
          <div style={{ marginTop: '16px', display: 'flex', flexDirection: 'column', gap: '12px' }}>
            <div>
              <strong>Date & Time</strong>
              <span>{new Date(appointment.appointmentDateTime).toLocaleString()}</span>
            </div>
            <div>
              <strong>Type</strong>
              <span>{getTypeString(appointment.type)}</span>
            </div>
            <div>
              <strong>Reason</strong>
              <span>{appointment.visitReason}</span>
            </div>
            <div>
              <strong>Notes</strong>
              <span>{appointment.notes || 'No notes provided.'}</span>
            </div>
            <div>
              <strong>Patient ID</strong>
              <span>{appointment.patientId}</span>
            </div>
            <div>
              <strong>Provider ID</strong>
              <span>{appointment.providerId}</span>
            </div>
          </div>
        </div>

        <div className="info-panel">
          <h3>Lifecycle Workflow</h3>
          <p style={{ fontSize: '0.9rem', color: '#62707d', marginBottom: '16px' }}>Manage the current status of the appointment workflow below.</p>
          
          <div style={{ display: 'flex', flexDirection: 'column', gap: '12px' }}>
            {appointment.status === AppointmentStatus.Requested && (
              <>
                <button disabled={updating} onClick={() => handleStatusUpdate(AppointmentStatus.Scheduled)} style={{ padding: '8px', background: '#e8f7f0', color: '#167455', border: '1px solid #167455', borderRadius: '4px', cursor: 'pointer', fontWeight: 600 }}>Approve & Schedule</button>
                <button disabled={updating} onClick={() => handleStatusUpdate(AppointmentStatus.Cancelled)} style={{ padding: '8px', background: '#fed7d7', color: '#822727', border: '1px solid #822727', borderRadius: '4px', cursor: 'pointer', fontWeight: 600 }}>Cancel Request</button>
              </>
            )}

            {appointment.status === AppointmentStatus.Scheduled && (
              <>
                <button disabled={updating} onClick={() => handleStatusUpdate(AppointmentStatus.CheckedIn)} style={{ padding: '8px', background: '#faf089', color: '#744210', border: '1px solid #744210', borderRadius: '4px', cursor: 'pointer', fontWeight: 600 }}>Patient Checked In</button>
                <button disabled={updating} onClick={() => handleStatusUpdate(AppointmentStatus.NoShow)} style={{ padding: '8px', background: '#e2e8f0', color: '#4a5568', border: '1px solid #4a5568', borderRadius: '4px', cursor: 'pointer', fontWeight: 600 }}>Mark No Show</button>
                <button disabled={updating} onClick={() => handleStatusUpdate(AppointmentStatus.Cancelled)} style={{ padding: '8px', background: '#fed7d7', color: '#822727', border: '1px solid #822727', borderRadius: '4px', cursor: 'pointer', fontWeight: 600 }}>Cancel Appointment</button>
              </>
            )}

            {appointment.status === AppointmentStatus.CheckedIn && (
              <>
                <button disabled={updating} onClick={() => handleStatusUpdate(AppointmentStatus.Completed)} style={{ padding: '8px', background: '#c6f6d5', color: '#22543d', border: '1px solid #22543d', borderRadius: '4px', cursor: 'pointer', fontWeight: 600 }}>Mark Completed</button>
                <button disabled={updating} onClick={() => handleStatusUpdate(AppointmentStatus.Cancelled)} style={{ padding: '8px', background: '#fed7d7', color: '#822727', border: '1px solid #822727', borderRadius: '4px', cursor: 'pointer', fontWeight: 600 }}>Cancel Appointment</button>
              </>
            )}

            {(appointment.status === AppointmentStatus.Completed || 
              appointment.status === AppointmentStatus.Cancelled || 
              appointment.status === AppointmentStatus.NoShow) && (
              <p style={{ fontStyle: 'italic', color: '#62707d' }}>This appointment has reached a final state.</p>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

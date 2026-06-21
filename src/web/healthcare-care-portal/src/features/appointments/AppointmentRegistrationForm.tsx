import React, { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { useNavigate } from 'react-router-dom';
import { appointmentApi } from '../../core/api/appointmentApi';
import { patientApi } from '../../core/api/patientApi';
import { providerApi } from '../../core/api/providerApi';
import { AppointmentType } from './types';
import { Patient } from '../patients/types';
import { Provider } from '../providers/types';

const appointmentSchema = z.object({
  patientId: z.string().min(1, 'Patient is required'),
  providerId: z.string().min(1, 'Provider is required'),
  appointmentDateTime: z.string().min(1, 'Date and time are required'),
  visitReason: z.string().min(1, 'Reason is required').max(500, 'Max 500 characters'),
  type: z.nativeEnum(AppointmentType),
  notes: z.string().max(2000, 'Max 2000 characters').optional()
});

type AppointmentFormData = z.infer<typeof appointmentSchema>;

export const AppointmentRegistrationForm: React.FC = () => {
  const navigate = useNavigate();
  const [apiError, setApiError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const [patients, setPatients] = useState<Patient[]>([]);
  const [providers, setProviders] = useState<Provider[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [pats, provs] = await Promise.all([
          patientApi.getPatients(),
          providerApi.getProviders()
        ]);
        setPatients(pats);
        setProviders(provs);
      } catch (err) {
        setApiError('Failed to load lookup data for patients/providers.');
      }
    };
    fetchData();
  }, []);

  const { register, handleSubmit, formState: { errors } } = useForm<AppointmentFormData>({
    resolver: zodResolver(appointmentSchema),
    defaultValues: {
      type: AppointmentType.Consultation
    }
  });

  const onSubmit = async (data: AppointmentFormData) => {
    setApiError(null);
    setIsSubmitting(true);
    try {
      // Append a time zone to native datetime-local for proper parsing, or keep it basic for demo
      const isoDate = new Date(data.appointmentDateTime).toISOString();
      const newAppointment = await appointmentApi.scheduleAppointment({
        ...data,
        appointmentDateTime: isoDate,
        type: Number(data.type)
      });
      navigate(`/appointments/${newAppointment.id}`);
    } catch (err: any) {
      setApiError(err.response?.data?.detail || 'Failed to schedule appointment. Ensure the date is in the future.');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="page-section">
      <div className="section-header">
        <h2>Schedule Appointment</h2>
      </div>
      <div className="info-panel" style={{ marginTop: '20px' }}>
        {apiError && <div style={{ color: 'red', marginBottom: '16px', padding: '12px', background: '#ffe6e6', borderRadius: '4px' }}>{apiError}</div>}
        
        <form onSubmit={handleSubmit(onSubmit)} style={{ display: 'flex', flexDirection: 'column', gap: '16px', maxWidth: '600px' }}>
          
          <div>
            <label style={{ display: 'block', fontWeight: 600, marginBottom: '4px' }}>Patient *</label>
            <select {...register('patientId')} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }}>
              <option value="">-- Select Patient --</option>
              {patients.map(p => (
                <option key={p.id} value={p.id}>{p.fullName}</option>
              ))}
            </select>
            {errors.patientId && <span style={{ color: 'red', fontSize: '0.85rem' }}>{errors.patientId.message}</span>}
          </div>

          <div>
            <label style={{ display: 'block', fontWeight: 600, marginBottom: '4px' }}>Provider *</label>
            <select {...register('providerId')} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }}>
              <option value="">-- Select Provider --</option>
              {providers.map(p => (
                <option key={p.id} value={p.id}>{p.fullName} - {p.specialty === 0 ? 'General Medicine' : 'Specialist'}</option>
              ))}
            </select>
            {errors.providerId && <span style={{ color: 'red', fontSize: '0.85rem' }}>{errors.providerId.message}</span>}
          </div>

          <div>
            <label style={{ display: 'block', fontWeight: 600, marginBottom: '4px' }}>Date and Time *</label>
            <input type="datetime-local" {...register('appointmentDateTime')} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }} />
            {errors.appointmentDateTime && <span style={{ color: 'red', fontSize: '0.85rem' }}>{errors.appointmentDateTime.message}</span>}
          </div>

          <div>
            <label style={{ display: 'block', fontWeight: 600, marginBottom: '4px' }}>Appointment Type</label>
            <select {...register('type')} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }}>
              <option value={AppointmentType.Consultation}>Consultation</option>
              <option value={AppointmentType.FollowUp}>Follow Up</option>
              <option value={AppointmentType.LabReview}>Lab Review</option>
              <option value={AppointmentType.MedicationReview}>Medication Review</option>
              <option value={AppointmentType.CarePlanReview}>Care Plan Review</option>
            </select>
            {errors.type && <span style={{ color: 'red', fontSize: '0.85rem' }}>{errors.type.message}</span>}
          </div>

          <div>
            <label style={{ display: 'block', fontWeight: 600, marginBottom: '4px' }}>Visit Reason *</label>
            <input type="text" {...register('visitReason')} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }} />
            {errors.visitReason && <span style={{ color: 'red', fontSize: '0.85rem' }}>{errors.visitReason.message}</span>}
          </div>

          <div>
            <label style={{ display: 'block', fontWeight: 600, marginBottom: '4px' }}>Notes</label>
            <textarea {...register('notes')} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc', minHeight: '80px' }} />
            {errors.notes && <span style={{ color: 'red', fontSize: '0.85rem' }}>{errors.notes.message}</span>}
          </div>

          <div style={{ marginTop: '20px' }}>
            <button type="submit" disabled={isSubmitting} style={{ background: '#21a67a', color: 'white', padding: '10px 24px', border: 'none', borderRadius: '4px', fontWeight: 700, cursor: isSubmitting ? 'not-allowed' : 'pointer', opacity: isSubmitting ? 0.7 : 1 }}>
              {isSubmitting ? 'Scheduling...' : 'Schedule Appointment'}
            </button>
            <button type="button" onClick={() => navigate('/appointments')} disabled={isSubmitting} style={{ background: 'transparent', color: '#62707d', padding: '10px 24px', border: '1px solid #d9e4e7', borderRadius: '4px', fontWeight: 700, marginLeft: '12px', cursor: 'pointer' }}>
              Cancel
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

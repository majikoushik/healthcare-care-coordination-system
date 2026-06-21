import React, { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { useNavigate } from 'react-router-dom';
import { carePlanApi } from '../../core/api/carePlanApi';
import { patientApi } from '../../core/api/patientApi';
import { providerApi } from '../../core/api/providerApi';
import { Patient } from '../patients/types';
import { Provider } from '../providers/types';

const carePlanSchema = z.object({
  patientId: z.string().min(1, 'Patient is required'),
  providerId: z.string().min(1, 'Provider is required'),
  title: z.string().min(1, 'Title is required').max(200, 'Max 200 characters'),
  clinicalSummary: z.string().min(1, 'Clinical summary is required'),
  instructions: z.string().min(1, 'Instructions are required'),
  followUpDate: z.string().optional()
});

type CarePlanFormData = z.infer<typeof carePlanSchema>;

export const CarePlanRegistrationForm: React.FC = () => {
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

  const { register, handleSubmit, formState: { errors } } = useForm<CarePlanFormData>({
    resolver: zodResolver(carePlanSchema)
  });

  const onSubmit = async (data: CarePlanFormData) => {
    setApiError(null);
    setIsSubmitting(true);
    try {
      const isoDate = data.followUpDate ? new Date(data.followUpDate).toISOString() : undefined;
      const newPlan = await carePlanApi.createCarePlan({
        ...data,
        followUpDate: isoDate,
        goals: [], // Initialize with empty lists for MVP
        tasks: []
      });
      navigate(`/care-plans/${newPlan.id}`);
    } catch (err: any) {
      setApiError(err.response?.data?.detail || 'Failed to create care plan. Ensure dates are valid.');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="page-section">
      <div className="section-header">
        <h2>Create Care Plan</h2>
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
                <option key={p.id} value={p.id}>{p.fullName}</option>
              ))}
            </select>
            {errors.providerId && <span style={{ color: 'red', fontSize: '0.85rem' }}>{errors.providerId.message}</span>}
          </div>

          <div>
            <label style={{ display: 'block', fontWeight: 600, marginBottom: '4px' }}>Title *</label>
            <input type="text" placeholder="e.g., Diabetes Follow-up Care Plan" {...register('title')} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }} />
            {errors.title && <span style={{ color: 'red', fontSize: '0.85rem' }}>{errors.title.message}</span>}
          </div>

          <div>
            <label style={{ display: 'block', fontWeight: 600, marginBottom: '4px' }}>Clinical Summary * (Demo Content Only)</label>
            <textarea placeholder="Synthetic demo summary..." {...register('clinicalSummary')} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc', minHeight: '80px' }} />
            {errors.clinicalSummary && <span style={{ color: 'red', fontSize: '0.85rem' }}>{errors.clinicalSummary.message}</span>}
          </div>

          <div>
            <label style={{ display: 'block', fontWeight: 600, marginBottom: '4px' }}>Instructions *</label>
            <textarea placeholder="e.g., Monitor symptoms, complete follow-up lab test..." {...register('instructions')} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc', minHeight: '80px' }} />
            {errors.instructions && <span style={{ color: 'red', fontSize: '0.85rem' }}>{errors.instructions.message}</span>}
          </div>

          <div>
            <label style={{ display: 'block', fontWeight: 600, marginBottom: '4px' }}>Follow-up Date</label>
            <input type="date" {...register('followUpDate')} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }} />
            {errors.followUpDate && <span style={{ color: 'red', fontSize: '0.85rem' }}>{errors.followUpDate.message}</span>}
          </div>

          <div style={{ marginTop: '20px' }}>
            <button type="submit" disabled={isSubmitting} style={{ background: '#21a67a', color: 'white', padding: '10px 24px', border: 'none', borderRadius: '4px', fontWeight: 700, cursor: isSubmitting ? 'not-allowed' : 'pointer', opacity: isSubmitting ? 0.7 : 1 }}>
              {isSubmitting ? 'Creating...' : 'Create Care Plan'}
            </button>
            <button type="button" onClick={() => navigate('/care-plans')} disabled={isSubmitting} style={{ background: 'transparent', color: '#62707d', padding: '10px 24px', border: '1px solid #d9e4e7', borderRadius: '4px', fontWeight: 700, marginLeft: '12px', cursor: 'pointer' }}>
              Cancel
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

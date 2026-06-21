import React, { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { useNavigate } from 'react-router-dom';
import { clinicalInsightApi } from '../../core/api/clinicalInsightApi';
import { patientApi } from '../../core/api/patientApi';
import { providerApi } from '../../core/api/providerApi';
import { Patient } from '../patients/types';
import { Provider } from '../providers/types';

const insightSchema = z.object({
  patientId: z.string().min(1, 'Patient is required'),
  providerId: z.string().min(1, 'Provider is required'),
  clinicalNoteText: z.string()
    .min(10, 'Clinical note text is required (min 10 chars)')
    .max(5000, 'Max 5000 characters for demo')
});

type InsightFormData = z.infer<typeof insightSchema>;

export const ClinicalInsightAnalysisForm: React.FC = () => {
  const navigate = useNavigate();
  const [apiError, setApiError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [providerStatus, setProviderStatus] = useState<any>(null);

  const [patients, setPatients] = useState<Patient[]>([]);
  const [providers, setProviders] = useState<Provider[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [pats, provs, status] = await Promise.all([
          patientApi.getPatients(),
          providerApi.getProviders(),
          clinicalInsightApi.getAiProviderStatus().catch(() => null)
        ]);
        setPatients(pats);
        setProviders(provs);
        setProviderStatus(status);
      } catch (err) {
        setApiError('Failed to load lookup data for patients/providers.');
      }
    };
    fetchData();
  }, []);

  const { register, handleSubmit, formState: { errors } } = useForm<InsightFormData>({
    resolver: zodResolver(insightSchema)
  });

  const onSubmit = async (data: InsightFormData) => {
    setApiError(null);
    setIsSubmitting(true);
    try {
      const insight = await clinicalInsightApi.analyzeNote(data);
      navigate(`/clinical-insights/${insight.id}`);
    } catch (err: any) {
      setApiError(err.response?.data?.detail || 'Failed to analyze clinical note.');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="page-section">
      <div className="section-header">
        <h2>Submit Clinical Note for Analysis</h2>
      </div>
      <div className="info-panel" style={{ marginTop: '20px' }}>
        <p style={{ color: '#62707d', marginBottom: '16px', fontSize: '0.9rem' }}>
          <strong>Responsible AI Notice:</strong> This feature uses a mocked AI provider locally. Do NOT paste real patient medical records. Synthetic data only. AI output requires human review.
        </p>

        {providerStatus && (
          <div style={{ background: providerStatus.providerMode === 'AzureAIConfigured' ? '#ebf8ff' : '#fdfaed', padding: '12px', borderRadius: '4px', border: providerStatus.providerMode === 'AzureAIConfigured' ? '1px solid #90cdf4' : '1px solid #feebc8', marginBottom: '16px', fontSize: '0.9rem' }}>
            <strong>AI Provider Readiness: </strong> 
            {providerStatus.message} (Using <code>{providerStatus.activeProvider}</code>)
          </div>
        )}

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
            <label style={{ display: 'block', fontWeight: 600, marginBottom: '4px' }}>Clinical Note Text * (Synthetic Data Only)</label>
            <textarea placeholder="Example: Patient presented with fatigue and poor diet. Blood sugar levels were elevated..." {...register('clinicalNoteText')} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc', minHeight: '150px' }} />
            {errors.clinicalNoteText && <span style={{ color: 'red', fontSize: '0.85rem' }}>{errors.clinicalNoteText.message}</span>}
          </div>

          <div style={{ marginTop: '20px' }}>
            <button type="submit" disabled={isSubmitting} style={{ background: '#21a67a', color: 'white', padding: '10px 24px', border: 'none', borderRadius: '4px', fontWeight: 700, cursor: isSubmitting ? 'not-allowed' : 'pointer', opacity: isSubmitting ? 0.7 : 1 }}>
              {isSubmitting ? 'Analyzing...' : 'Analyze Note'}
            </button>
            <button type="button" onClick={() => navigate('/clinical-insights')} disabled={isSubmitting} style={{ background: 'transparent', color: '#62707d', padding: '10px 24px', border: '1px solid #d9e4e7', borderRadius: '4px', fontWeight: 700, marginLeft: '12px', cursor: 'pointer' }}>
              Cancel
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

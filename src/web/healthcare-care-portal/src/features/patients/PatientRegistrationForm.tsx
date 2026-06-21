import React, { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { useNavigate } from 'react-router-dom';
import { patientApi } from '../../core/api/patientApi';
import { Gender, ConsentStatus } from './types';

const patientSchema = z.object({
  fullName: z.string().min(1, 'Full name is required').max(200, 'Max 200 characters'),
  dateOfBirth: z.string().min(1, 'Date of birth is required').refine(date => new Date(date) <= new Date(), { message: 'Date of birth cannot be in the future' }),
  gender: z.nativeEnum(Gender),
  email: z.string().min(1, 'Email is required').email('Invalid email format').max(150, 'Max 150 characters'),
  mobileNumber: z.string().min(1, 'Mobile number is required').max(50, 'Max 50 characters'),
  address: z.string().min(1, 'Address is required').max(500, 'Max 500 characters'),
  emergencyContactName: z.string().min(1, 'Emergency contact name is required').max(200, 'Max 200 characters'),
  emergencyContactNumber: z.string().min(1, 'Emergency contact number is required').max(50, 'Max 50 characters'),
  consentStatus: z.nativeEnum(ConsentStatus)
});

type PatientFormData = z.infer<typeof patientSchema>;

export const PatientRegistrationForm: React.FC = () => {
  const navigate = useNavigate();
  const [apiError, setApiError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const { register, handleSubmit, formState: { errors } } = useForm<PatientFormData>({
    resolver: zodResolver(patientSchema),
    defaultValues: {
      gender: Gender.PreferNotToSay,
      consentStatus: ConsentStatus.NotProvided
    }
  });

  const onSubmit = async (data: PatientFormData) => {
    setApiError(null);
    setIsSubmitting(true);
    try {
      const newPatient = await patientApi.registerPatient({
        ...data,
        gender: Number(data.gender),
        consentStatus: Number(data.consentStatus)
      });
      navigate(`/patients/${newPatient.id}`);
    } catch (err: any) {
      setApiError(err.response?.data?.detail || 'Failed to register patient. Please try again.');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="page-section">
      <div className="section-header">
        <h2>Register New Patient</h2>
      </div>
      <div className="info-panel" style={{ marginTop: '20px' }}>
        {apiError && <div style={{ color: 'red', marginBottom: '16px', padding: '12px', background: '#ffe6e6', borderRadius: '4px' }}>{apiError}</div>}
        
        <form onSubmit={handleSubmit(onSubmit)} style={{ display: 'flex', flexDirection: 'column', gap: '16px', maxWidth: '600px' }}>
          
          <div>
            <label style={{ display: 'block', fontWeight: 600, marginBottom: '4px' }}>Full Name *</label>
            <input type="text" {...register('fullName')} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }} />
            {errors.fullName && <span style={{ color: 'red', fontSize: '0.85rem' }}>{errors.fullName.message}</span>}
          </div>

          <div>
            <label style={{ display: 'block', fontWeight: 600, marginBottom: '4px' }}>Date of Birth *</label>
            <input type="date" {...register('dateOfBirth')} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }} />
            {errors.dateOfBirth && <span style={{ color: 'red', fontSize: '0.85rem' }}>{errors.dateOfBirth.message}</span>}
          </div>

          <div>
            <label style={{ display: 'block', fontWeight: 600, marginBottom: '4px' }}>Gender</label>
            <select {...register('gender')} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }}>
              <option value={Gender.PreferNotToSay}>Prefer not to say</option>
              <option value={Gender.Male}>Male</option>
              <option value={Gender.Female}>Female</option>
              <option value={Gender.Other}>Other</option>
            </select>
            {errors.gender && <span style={{ color: 'red', fontSize: '0.85rem' }}>{errors.gender.message}</span>}
          </div>

          <div>
            <label style={{ display: 'block', fontWeight: 600, marginBottom: '4px' }}>Email *</label>
            <input type="email" {...register('email')} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }} />
            {errors.email && <span style={{ color: 'red', fontSize: '0.85rem' }}>{errors.email.message}</span>}
          </div>

          <div>
            <label style={{ display: 'block', fontWeight: 600, marginBottom: '4px' }}>Mobile Number *</label>
            <input type="text" {...register('mobileNumber')} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }} />
            {errors.mobileNumber && <span style={{ color: 'red', fontSize: '0.85rem' }}>{errors.mobileNumber.message}</span>}
          </div>

          <div>
            <label style={{ display: 'block', fontWeight: 600, marginBottom: '4px' }}>Address *</label>
            <textarea {...register('address')} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc', minHeight: '80px' }} />
            {errors.address && <span style={{ color: 'red', fontSize: '0.85rem' }}>{errors.address.message}</span>}
          </div>

          <fieldset style={{ border: '1px solid #d9e4e7', padding: '16px', borderRadius: '4px', marginTop: '8px' }}>
            <legend style={{ fontWeight: 600, padding: '0 8px' }}>Emergency Contact</legend>
            <div style={{ marginBottom: '12px' }}>
              <label style={{ display: 'block', fontWeight: 600, marginBottom: '4px' }}>Name *</label>
              <input type="text" {...register('emergencyContactName')} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }} />
              {errors.emergencyContactName && <span style={{ color: 'red', fontSize: '0.85rem' }}>{errors.emergencyContactName.message}</span>}
            </div>
            <div>
              <label style={{ display: 'block', fontWeight: 600, marginBottom: '4px' }}>Number *</label>
              <input type="text" {...register('emergencyContactNumber')} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }} />
              {errors.emergencyContactNumber && <span style={{ color: 'red', fontSize: '0.85rem' }}>{errors.emergencyContactNumber.message}</span>}
            </div>
          </fieldset>

          <div>
            <label style={{ display: 'block', fontWeight: 600, marginBottom: '4px' }}>Consent Status</label>
            <select {...register('consentStatus')} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }}>
              <option value={ConsentStatus.NotProvided}>Not Provided</option>
              <option value={ConsentStatus.Provided}>Provided</option>
              <option value={ConsentStatus.Withdrawn}>Withdrawn</option>
            </select>
            {errors.consentStatus && <span style={{ color: 'red', fontSize: '0.85rem' }}>{errors.consentStatus.message}</span>}
          </div>

          <div style={{ marginTop: '20px' }}>
            <button type="submit" disabled={isSubmitting} style={{ background: '#21a67a', color: 'white', padding: '10px 24px', border: 'none', borderRadius: '4px', fontWeight: 700, cursor: isSubmitting ? 'not-allowed' : 'pointer', opacity: isSubmitting ? 0.7 : 1 }}>
              {isSubmitting ? 'Registering...' : 'Register Patient'}
            </button>
            <button type="button" onClick={() => navigate('/patients')} disabled={isSubmitting} style={{ background: 'transparent', color: '#62707d', padding: '10px 24px', border: '1px solid #d9e4e7', borderRadius: '4px', fontWeight: 700, marginLeft: '12px', cursor: 'pointer' }}>
              Cancel
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

import React, { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { useNavigate } from 'react-router-dom';
import { providerApi } from '../../core/api/providerApi';
import { Specialty, AvailabilityStatus } from './types';

const providerSchema = z.object({
  fullName: z.string().min(1, 'Full name is required').max(200, 'Max 200 characters'),
  specialty: z.nativeEnum(Specialty),
  email: z.string().min(1, 'Email is required').email('Invalid email format').max(150, 'Max 150 characters'),
  mobileNumber: z.string().min(1, 'Mobile number is required').max(50, 'Max 50 characters'),
  department: z.string().min(1, 'Department is required').max(200, 'Max 200 characters'),
  availabilityStatus: z.nativeEnum(AvailabilityStatus)
});

type ProviderFormData = z.infer<typeof providerSchema>;

export const ProviderRegistrationForm: React.FC = () => {
  const navigate = useNavigate();
  const [apiError, setApiError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const { register, handleSubmit, formState: { errors } } = useForm<ProviderFormData>({
    resolver: zodResolver(providerSchema),
    defaultValues: {
      specialty: Specialty.GeneralMedicine,
      availabilityStatus: AvailabilityStatus.Available
    }
  });

  const onSubmit = async (data: ProviderFormData) => {
    setApiError(null);
    setIsSubmitting(true);
    try {
      const newProvider = await providerApi.registerProvider({
        ...data,
        specialty: Number(data.specialty),
        availabilityStatus: Number(data.availabilityStatus)
      });
      navigate(`/providers/${newProvider.id}`);
    } catch (err: any) {
      setApiError(err.response?.data?.detail || 'Failed to register provider. Please try again.');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="page-section">
      <div className="section-header">
        <h2>Register New Provider</h2>
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
            <label style={{ display: 'block', fontWeight: 600, marginBottom: '4px' }}>Specialty</label>
            <select {...register('specialty')} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }}>
              <option value={Specialty.GeneralMedicine}>General Medicine</option>
              <option value={Specialty.Cardiology}>Cardiology</option>
              <option value={Specialty.Endocrinology}>Endocrinology</option>
              <option value={Specialty.Orthopedics}>Orthopedics</option>
              <option value={Specialty.Pediatrics}>Pediatrics</option>
              <option value={Specialty.Neurology}>Neurology</option>
              <option value={Specialty.Dermatology}>Dermatology</option>
            </select>
            {errors.specialty && <span style={{ color: 'red', fontSize: '0.85rem' }}>{errors.specialty.message}</span>}
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
            <label style={{ display: 'block', fontWeight: 600, marginBottom: '4px' }}>Department *</label>
            <input type="text" {...register('department')} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }} />
            {errors.department && <span style={{ color: 'red', fontSize: '0.85rem' }}>{errors.department.message}</span>}
          </div>

          <div>
            <label style={{ display: 'block', fontWeight: 600, marginBottom: '4px' }}>Availability Status</label>
            <select {...register('availabilityStatus')} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }}>
              <option value={AvailabilityStatus.Available}>Available</option>
              <option value={AvailabilityStatus.Busy}>Busy</option>
              <option value={AvailabilityStatus.OnLeave}>On Leave</option>
              <option value={AvailabilityStatus.Inactive}>Inactive</option>
            </select>
            {errors.availabilityStatus && <span style={{ color: 'red', fontSize: '0.85rem' }}>{errors.availabilityStatus.message}</span>}
          </div>

          <div style={{ marginTop: '20px' }}>
            <button type="submit" disabled={isSubmitting} style={{ background: '#21a67a', color: 'white', padding: '10px 24px', border: 'none', borderRadius: '4px', fontWeight: 700, cursor: isSubmitting ? 'not-allowed' : 'pointer', opacity: isSubmitting ? 0.7 : 1 }}>
              {isSubmitting ? 'Registering...' : 'Register Provider'}
            </button>
            <button type="button" onClick={() => navigate('/providers')} disabled={isSubmitting} style={{ background: 'transparent', color: '#62707d', padding: '10px 24px', border: '1px solid #d9e4e7', borderRadius: '4px', fontWeight: 700, marginLeft: '12px', cursor: 'pointer' }}>
              Cancel
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

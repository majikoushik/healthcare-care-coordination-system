import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { notificationApi } from '../../core/api/notificationApi';
import { NotificationChannel, RecipientType } from './types';

interface FormData {
  relatedEntityType: string;
  relatedEntityId: string;
  notificationType: string;
  channel: string;
  recipientType: string;
  recipientReference: string;
  subject: string;
  messageSummary: string;
}

export const NotificationSimulationForm: React.FC = () => {
  const { register, handleSubmit, formState: { errors } } = useForm<FormData>();
  const navigate = useNavigate();
  const [apiError, setApiError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);
  
  const onSubmit = async (data: FormData) => {
    try {
      setIsSubmitting(true);
      setApiError(null);
      
      const createdRecord = await notificationApi.createNotification({
        ...data,
        channel: parseInt(data.channel) as NotificationChannel,
        recipientType: parseInt(data.recipientType) as RecipientType
      });
      
      navigate(`/notifications/${createdRecord.id}`);
    } catch (err: any) {
      setApiError(err.response?.data?.detail || 'An error occurred while queueing the notification.');
      setIsSubmitting(false);
    }
  };

  return (
    <div style={{ padding: '24px', maxWidth: '600px', margin: '0 auto' }}>
      <h1 style={{ fontSize: '24px', fontWeight: 'bold', marginBottom: '24px' }}>Request Notification Simulation</h1>
      
      <div style={{ background: '#eebf2422', border: '1px solid #eebf24', padding: '16px', borderRadius: '8px', marginBottom: '24px' }}>
        <strong>Privacy Notice:</strong> Do not inject raw clinical notes into the message summary. Use generic templates (e.g. "Your lab results are ready").
      </div>

      {apiError && <div style={{ color: 'red', marginBottom: '16px', padding: '12px', background: '#ffe6e6', borderRadius: '4px' }}>{apiError}</div>}
      
      <form onSubmit={handleSubmit(onSubmit)} style={{ display: 'flex', flexDirection: 'column', gap: '16px' }}>
        <div style={{ display: 'flex', gap: '16px' }}>
          <div style={{ flex: 1 }}>
            <label style={{ display: 'block', marginBottom: '8px', fontWeight: 'bold' }}>Related Entity Type *</label>
            <select 
              {...register('relatedEntityType', { required: 'Required' })}
              style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #cbd5e0' }}
            >
              <option value="Appointment">Appointment</option>
              <option value="CarePlan">CarePlan</option>
              <option value="FollowUpTask">FollowUpTask</option>
              <option value="ClinicalInsight">ClinicalInsight</option>
              <option value="Patient">Patient</option>
            </select>
          </div>
          
          <div style={{ flex: 1 }}>
            <label style={{ display: 'block', marginBottom: '8px', fontWeight: 'bold' }}>Related Entity ID *</label>
            <input 
              {...register('relatedEntityId', { required: 'Required' })}
              style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #cbd5e0' }}
              placeholder="e.g., GUID or string ID"
            />
            {errors.relatedEntityId && <span style={{ color: 'red', fontSize: '12px' }}>{errors.relatedEntityId.message}</span>}
          </div>
        </div>

        <div>
          <label style={{ display: 'block', marginBottom: '8px', fontWeight: 'bold' }}>Notification Type *</label>
          <select 
              {...register('notificationType', { required: 'Required' })}
              style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #cbd5e0' }}
            >
              <option value="AppointmentScheduled">AppointmentScheduled</option>
              <option value="CarePlanUpdated">CarePlanUpdated</option>
              <option value="FollowUpTaskDue">FollowUpTaskDue</option>
              <option value="ClinicalInsightReviewRequired">ClinicalInsightReviewRequired</option>
              <option value="SystemAlert">SystemAlert</option>
            </select>
        </div>

        <div style={{ display: 'flex', gap: '16px' }}>
          <div style={{ flex: 1 }}>
            <label style={{ display: 'block', marginBottom: '8px', fontWeight: 'bold' }}>Channel *</label>
            <select 
              {...register('channel', { required: 'Required' })}
              style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #cbd5e0' }}
            >
              <option value="0">Email (Simulation)</option>
              <option value="1">SMS (Simulation)</option>
              <option value="2">Portal Push (Simulation)</option>
            </select>
          </div>
          
          <div style={{ flex: 1 }}>
            <label style={{ display: 'block', marginBottom: '8px', fontWeight: 'bold' }}>Recipient Type *</label>
            <select 
              {...register('recipientType', { required: 'Required' })}
              style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #cbd5e0' }}
              defaultValue="0"
            >
              <option value="0">Patient</option>
              <option value="1">Provider</option>
              <option value="2">Care Coordinator</option>
            </select>
          </div>
        </div>

        <div>
          <label style={{ display: 'block', marginBottom: '8px', fontWeight: 'bold' }}>Recipient Address/Phone *</label>
          <input 
            {...register('recipientReference', { required: 'Recipient reference is required' })}
            style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #cbd5e0' }}
            placeholder="e.g., patient@example.com or 555-0199"
          />
          {errors.recipientReference && <span style={{ color: 'red', fontSize: '12px' }}>{errors.recipientReference.message}</span>}
        </div>

        <div>
          <label style={{ display: 'block', marginBottom: '8px', fontWeight: 'bold' }}>Subject *</label>
          <input 
            {...register('subject', { required: 'Subject is required' })}
            style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #cbd5e0' }}
            placeholder="e.g., Your appointment is scheduled"
          />
        </div>

        <div>
          <label style={{ display: 'block', marginBottom: '8px', fontWeight: 'bold' }}>Message Summary (Safe Template) *</label>
          <textarea 
            {...register('messageSummary', { required: 'Message summary is required' })}
            style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #cbd5e0', minHeight: '80px' }}
            placeholder="A safe summary of the event without containing raw clinical details..."
          />
        </div>

        <div style={{ display: 'flex', gap: '16px', marginTop: '16px' }}>
          <button 
            type="submit" 
            disabled={isSubmitting}
            style={{ background: '#3182ce', color: 'white', padding: '12px 24px', borderRadius: '4px', border: 'none', cursor: isSubmitting ? 'not-allowed' : 'pointer', fontWeight: 'bold', flex: 1 }}
          >
            {isSubmitting ? 'Queueing...' : 'Queue Notification'}
          </button>
          <button 
            type="button" 
            onClick={() => navigate('/notifications')}
            style={{ background: '#e2e8f0', color: '#4a5568', padding: '12px 24px', borderRadius: '4px', border: 'none', cursor: 'pointer', fontWeight: 'bold' }}
          >
            Cancel
          </button>
        </div>
      </form>
    </div>
  );
};

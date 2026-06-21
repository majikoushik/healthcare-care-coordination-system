import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { followUpTaskApi } from '../../core/api/followUpTaskApi';
import { patientApi } from '../../core/api/patientApi';
import { carePlanApi } from '../../core/api/carePlanApi';
import { Patient } from '../patients/types';
import { CarePlanDocument } from '../care-plans/types';
import { FollowUpTaskType, Priority } from './types';

interface FormData {
  patientId: string;
  carePlanId: string;
  title: string;
  description: string;
  taskType: string;
  priority: string;
  dueDate: string;
  assignedTo: string;
}

export const FollowUpTaskRegistrationForm: React.FC = () => {
  const { register, handleSubmit, formState: { errors }, watch } = useForm<FormData>();
  const navigate = useNavigate();
  const [apiError, setApiError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);
  
  const [patients, setPatients] = useState<Patient[]>([]);
  const [carePlans, setCarePlans] = useState<CarePlanDocument[]>([]);
  
  const selectedPatientId = watch('patientId');

  useEffect(() => {
    patientApi.getPatients().then(setPatients).catch(() => setApiError('Failed to load patients.'));
  }, []);

  useEffect(() => {
    if (selectedPatientId) {
      carePlanApi.getCarePlansByPatientId(selectedPatientId).then(setCarePlans).catch(() => setApiError('Failed to load care plans.'));
    } else {
      setCarePlans([]);
    }
  }, [selectedPatientId]);

  const onSubmit = async (data: FormData) => {
    try {
      setIsSubmitting(true);
      setApiError(null);
      
      const createdTask = await followUpTaskApi.createTask({
        patientId: data.patientId,
        carePlanId: data.carePlanId,
        title: data.title,
        description: data.description,
        taskType: parseInt(data.taskType) as FollowUpTaskType,
        priority: parseInt(data.priority) as Priority,
        dueDate: new Date(data.dueDate).toISOString(),
        assignedTo: data.assignedTo
      });
      
      navigate(`/follow-up-tasks/${createdTask.id}`);
    } catch (err: any) {
      setApiError(err.response?.data?.detail || 'An error occurred while creating the task.');
      setIsSubmitting(false);
    }
  };

  return (
    <div style={{ padding: '24px', maxWidth: '600px', margin: '0 auto' }}>
      <h1 style={{ fontSize: '24px', fontWeight: 'bold', marginBottom: '24px' }}>Create Follow-up Task</h1>
      
      {apiError && <div style={{ color: 'red', marginBottom: '16px', padding: '12px', background: '#ffe6e6', borderRadius: '4px' }}>{apiError}</div>}
      
      <form onSubmit={handleSubmit(onSubmit)} style={{ display: 'flex', flexDirection: 'column', gap: '16px' }}>
        <div>
          <label style={{ display: 'block', marginBottom: '8px', fontWeight: 'bold' }}>Patient *</label>
          <select 
            {...register('patientId', { required: 'Patient is required' })}
            style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #cbd5e0' }}
          >
            <option value="">Select Patient...</option>
            {patients.map(p => (
              <option key={p.id} value={p.id}>{p.fullName}</option>
            ))}
          </select>
          {errors.patientId && <span style={{ color: 'red', fontSize: '12px' }}>{errors.patientId.message}</span>}
        </div>

        <div>
          <label style={{ display: 'block', marginBottom: '8px', fontWeight: 'bold' }}>Care Plan *</label>
          <select 
            {...register('carePlanId', { required: 'Care Plan is required' })}
            disabled={!selectedPatientId || carePlans.length === 0}
            style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #cbd5e0' }}
          >
            <option value="">Select Care Plan...</option>
            {carePlans.map(cp => (
              <option key={cp.id} value={cp.id}>{cp.title}</option>
            ))}
          </select>
          {errors.carePlanId && <span style={{ color: 'red', fontSize: '12px' }}>{errors.carePlanId.message}</span>}
          {selectedPatientId && carePlans.length === 0 && <span style={{ color: '#718096', fontSize: '12px', display: 'block', marginTop: '4px' }}>This patient has no care plans. A task requires a care plan.</span>}
        </div>

        <div>
          <label style={{ display: 'block', marginBottom: '8px', fontWeight: 'bold' }}>Task Title *</label>
          <input 
            {...register('title', { required: 'Title is required', maxLength: 200 })}
            style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #cbd5e0' }}
            placeholder="e.g., Schedule MRI"
          />
          {errors.title && <span style={{ color: 'red', fontSize: '12px' }}>{errors.title.message}</span>}
        </div>

        <div>
          <label style={{ display: 'block', marginBottom: '8px', fontWeight: 'bold' }}>Description *</label>
          <textarea 
            {...register('description', { required: 'Description is required' })}
            style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #cbd5e0', minHeight: '100px' }}
            placeholder="Detailed instructions for the task..."
          />
          {errors.description && <span style={{ color: 'red', fontSize: '12px' }}>{errors.description.message}</span>}
        </div>

        <div style={{ display: 'flex', gap: '16px' }}>
          <div style={{ flex: 1 }}>
            <label style={{ display: 'block', marginBottom: '8px', fontWeight: 'bold' }}>Task Type *</label>
            <select 
              {...register('taskType', { required: 'Task Type is required' })}
              style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #cbd5e0' }}
            >
              <option value="0">General</option>
              <option value="1">Lab Test</option>
              <option value="2">Medication Review</option>
              <option value="3">Follow-up Appointment</option>
              <option value="4">Lifestyle Counseling</option>
              <option value="5">Care Plan Review</option>
              <option value="6">Patient Education</option>
            </select>
          </div>
          
          <div style={{ flex: 1 }}>
            <label style={{ display: 'block', marginBottom: '8px', fontWeight: 'bold' }}>Priority *</label>
            <select 
              {...register('priority', { required: 'Priority is required' })}
              style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #cbd5e0' }}
              defaultValue="1"
            >
              <option value="0">Low</option>
              <option value="1">Medium</option>
              <option value="2">High</option>
              <option value="3">Critical</option>
            </select>
          </div>
        </div>

        <div style={{ display: 'flex', gap: '16px' }}>
          <div style={{ flex: 1 }}>
            <label style={{ display: 'block', marginBottom: '8px', fontWeight: 'bold' }}>Due Date *</label>
            <input 
              type="date"
              {...register('dueDate', { required: 'Due date is required' })}
              style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #cbd5e0' }}
            />
            {errors.dueDate && <span style={{ color: 'red', fontSize: '12px' }}>{errors.dueDate.message}</span>}
          </div>
          
          <div style={{ flex: 1 }}>
            <label style={{ display: 'block', marginBottom: '8px', fontWeight: 'bold' }}>Assigned To *</label>
            <input 
              {...register('assignedTo', { required: 'Assignee is required' })}
              style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #cbd5e0' }}
              placeholder="e.g., CareCoordinator"
              defaultValue="CareCoordinator"
            />
            {errors.assignedTo && <span style={{ color: 'red', fontSize: '12px' }}>{errors.assignedTo.message}</span>}
          </div>
        </div>

        <div style={{ display: 'flex', gap: '16px', marginTop: '16px' }}>
          <button 
            type="submit" 
            disabled={isSubmitting}
            style={{ background: '#3182ce', color: 'white', padding: '12px 24px', borderRadius: '4px', border: 'none', cursor: isSubmitting ? 'not-allowed' : 'pointer', fontWeight: 'bold', flex: 1 }}
          >
            {isSubmitting ? 'Creating...' : 'Create Task'}
          </button>
          <button 
            type="button" 
            onClick={() => navigate('/follow-up-tasks')}
            style={{ background: '#e2e8f0', color: '#4a5568', padding: '12px 24px', borderRadius: '4px', border: 'none', cursor: 'pointer', fontWeight: 'bold' }}
          >
            Cancel
          </button>
        </div>
      </form>
    </div>
  );
};

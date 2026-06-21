import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { followUpTaskApi } from '../../core/api/followUpTaskApi';
import { FollowUpTask, FollowUpTaskStatus } from './types';

export const FollowUpTaskDetails: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [task, setTask] = useState<FollowUpTask | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [isUpdating, setIsUpdating] = useState(false);
  const [completionNotes, setCompletionNotes] = useState('');

  useEffect(() => {
    if (id) {
      fetchTask(id);
    }
  }, [id]);

  const fetchTask = async (taskId: string) => {
    try {
      setLoading(true);
      const data = await followUpTaskApi.getTaskById(taskId);
      setTask(data);
    } catch (err) {
      setError('Failed to fetch follow-up task details.');
    } finally {
      setLoading(false);
    }
  };

  const updateStatus = async (newStatus: FollowUpTaskStatus) => {
    if (!task) return;
    try {
      setIsUpdating(true);
      const updated = await followUpTaskApi.updateTaskStatus(task.id, {
        status: newStatus,
        updatedBy: 'CareCoordinator', // In a real app, this comes from auth context
        completionNotes: newStatus === FollowUpTaskStatus.Completed ? completionNotes : undefined
      });
      setTask(updated);
      setError(null);
    } catch (err) {
      setError('Failed to update task status.');
    } finally {
      setIsUpdating(false);
    }
  };

  if (loading) return <div>Loading task details...</div>;
  if (!task) return <div>Task not found</div>;

  return (
    <div style={{ padding: '24px', maxWidth: '800px', margin: '0 auto' }}>
      <button onClick={() => navigate('/follow-up-tasks')} style={{ marginBottom: '16px', background: 'none', border: 'none', color: '#3182ce', cursor: 'pointer', padding: 0 }}>
        &larr; Back to Tasks
      </button>

      {error && <div style={{ color: 'red', marginBottom: '16px', padding: '12px', background: '#ffe6e6', borderRadius: '4px' }}>{error}</div>}

      <div style={{ background: 'white', padding: '24px', borderRadius: '8px', boxShadow: '0 1px 3px rgba(0,0,0,0.1)' }}>
        <h1 style={{ fontSize: '24px', fontWeight: 'bold', marginBottom: '16px' }}>{task.title}</h1>
        
        <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px', marginBottom: '24px' }}>
          <div>
            <strong>Status:</strong> {FollowUpTaskStatus[task.status]}
          </div>
          <div>
            <strong>Priority:</strong> {task.priority}
          </div>
          <div>
            <strong>Due Date:</strong> {new Date(task.dueDate).toLocaleDateString()}
          </div>
          <div>
            <strong>Assigned To:</strong> {task.assignedTo}
          </div>
          <div>
            <strong>Patient ID:</strong> <span style={{ cursor: 'pointer', color: '#3182ce' }} onClick={() => navigate(`/patients/${task.patientId}`)}>{task.patientId}</span>
          </div>
          <div>
            <strong>Care Plan ID:</strong> <span style={{ cursor: 'pointer', color: '#3182ce' }} onClick={() => navigate(`/care-plans/${task.carePlanId}`)}>{task.carePlanId}</span>
          </div>
        </div>

        <div style={{ marginBottom: '24px' }}>
          <strong>Description:</strong>
          <p style={{ marginTop: '8px', background: '#f7fafc', padding: '16px', borderRadius: '4px' }}>{task.description}</p>
        </div>

        {task.status === FollowUpTaskStatus.Completed && task.completionNotes && (
          <div style={{ marginBottom: '24px' }}>
            <strong>Completion Notes:</strong>
            <p style={{ marginTop: '8px', background: '#f0fff4', padding: '16px', borderRadius: '4px', border: '1px solid #c6f6d5' }}>{task.completionNotes}</p>
          </div>
        )}

        <div style={{ borderTop: '1px solid #e2e8f0', paddingTop: '24px' }}>
          <h3 style={{ fontWeight: 'bold', marginBottom: '16px' }}>Actions</h3>
          
          <div style={{ display: 'flex', gap: '16px', alignItems: 'center' }}>
            {task.status === FollowUpTaskStatus.Pending && (
              <button 
                onClick={() => updateStatus(FollowUpTaskStatus.InProgress)}
                disabled={isUpdating}
                style={{ background: '#3182ce', color: 'white', padding: '8px 16px', borderRadius: '4px', border: 'none', cursor: 'pointer' }}
              >
                Mark In Progress
              </button>
            )}
            
            {(task.status === FollowUpTaskStatus.Pending || task.status === FollowUpTaskStatus.InProgress || task.status === FollowUpTaskStatus.Overdue) && (
              <div style={{ display: 'flex', gap: '8px', width: '100%' }}>
                <input 
                  type="text" 
                  placeholder="Completion notes (required for completion)" 
                  value={completionNotes}
                  onChange={(e) => setCompletionNotes(e.target.value)}
                  style={{ flex: 1, padding: '8px', borderRadius: '4px', border: '1px solid #cbd5e0' }}
                />
                <button 
                  onClick={() => updateStatus(FollowUpTaskStatus.Completed)}
                  disabled={isUpdating || !completionNotes}
                  style={{ background: '#38a169', color: 'white', padding: '8px 16px', borderRadius: '4px', border: 'none', cursor: completionNotes ? 'pointer' : 'not-allowed', opacity: completionNotes ? 1 : 0.5 }}
                >
                  Complete Task
                </button>
              </div>
            )}
            
            {(task.status === FollowUpTaskStatus.Pending || task.status === FollowUpTaskStatus.InProgress) && (
              <button 
                onClick={() => updateStatus(FollowUpTaskStatus.Cancelled)}
                disabled={isUpdating}
                style={{ background: '#e53e3e', color: 'white', padding: '8px 16px', borderRadius: '4px', border: 'none', cursor: 'pointer' }}
              >
                Cancel Task
              </button>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

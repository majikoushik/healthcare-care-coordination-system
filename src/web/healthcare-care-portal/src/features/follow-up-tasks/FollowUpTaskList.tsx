import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { followUpTaskApi } from '../../core/api/followUpTaskApi';
import { FollowUpTask, FollowUpTaskStatus, Priority } from './types';

export const FollowUpTaskList: React.FC = () => {
  const [tasks, setTasks] = useState<FollowUpTask[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [filter, setFilter] = useState<'all' | 'due-today' | 'overdue'>('all');
  const navigate = useNavigate();

  useEffect(() => {
    fetchTasks();
  }, [filter]);

  const fetchTasks = async () => {
    try {
      setLoading(true);
      let data: FollowUpTask[] = [];
      if (filter === 'all') {
        data = await followUpTaskApi.getTasks();
      } else if (filter === 'due-today') {
        data = await followUpTaskApi.getDueToday();
      } else if (filter === 'overdue') {
        data = await followUpTaskApi.getOverdue();
      }
      setTasks(data);
      setError(null);
    } catch (err) {
      setError('Failed to fetch follow-up tasks. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const getStatusBadge = (status: FollowUpTaskStatus) => {
    switch (status) {
      case FollowUpTaskStatus.Pending: return <span style={{ background: '#edf2f7', padding: '4px 8px', borderRadius: '4px' }}>Pending</span>;
      case FollowUpTaskStatus.InProgress: return <span style={{ background: '#ebf4ff', color: '#3182ce', padding: '4px 8px', borderRadius: '4px' }}>In Progress</span>;
      case FollowUpTaskStatus.Completed: return <span style={{ background: '#f0fff4', color: '#38a169', padding: '4px 8px', borderRadius: '4px' }}>Completed</span>;
      case FollowUpTaskStatus.Overdue: return <span style={{ background: '#fff5f5', color: '#e53e3e', padding: '4px 8px', borderRadius: '4px' }}>Overdue</span>;
      case FollowUpTaskStatus.Cancelled: return <span style={{ background: '#e2e8f0', color: '#718096', padding: '4px 8px', borderRadius: '4px' }}>Cancelled</span>;
      default: return null;
    }
  };

  const getPriorityBadge = (priority: Priority) => {
    switch (priority) {
      case Priority.Critical: return <span style={{ background: '#fed7d7', color: '#c53030', padding: '4px 8px', borderRadius: '4px', fontSize: '0.8rem' }}>Critical</span>;
      case Priority.High: return <span style={{ background: '#feebc8', color: '#dd6b20', padding: '4px 8px', borderRadius: '4px', fontSize: '0.8rem' }}>High</span>;
      case Priority.Medium: return <span style={{ background: '#e2e8f0', color: '#4a5568', padding: '4px 8px', borderRadius: '4px', fontSize: '0.8rem' }}>Medium</span>;
      case Priority.Low: return <span style={{ background: '#edf2f7', color: '#718096', padding: '4px 8px', borderRadius: '4px', fontSize: '0.8rem' }}>Low</span>;
      default: return null;
    }
  };

  if (loading) return <div>Loading tasks...</div>;

  return (
    <div style={{ padding: '24px' }}>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '24px' }}>
        <h1 style={{ fontSize: '24px', fontWeight: 'bold' }}>Follow-up Tasks</h1>
        <button 
          onClick={() => navigate('/follow-up-tasks/new')}
          style={{ background: '#3182ce', color: 'white', padding: '8px 16px', borderRadius: '4px', border: 'none', cursor: 'pointer' }}
        >
          Create Task
        </button>
      </div>

      <div style={{ marginBottom: '16px', display: 'flex', gap: '8px' }}>
        <button onClick={() => setFilter('all')} style={{ fontWeight: filter === 'all' ? 'bold' : 'normal', padding: '8px', cursor: 'pointer' }}>All</button>
        <button onClick={() => setFilter('due-today')} style={{ fontWeight: filter === 'due-today' ? 'bold' : 'normal', padding: '8px', cursor: 'pointer' }}>Due Today</button>
        <button onClick={() => setFilter('overdue')} style={{ fontWeight: filter === 'overdue' ? 'bold' : 'normal', padding: '8px', cursor: 'pointer' }}>Overdue</button>
      </div>

      {error && <div style={{ color: 'red', marginBottom: '16px' }}>{error}</div>}

      {tasks.length === 0 ? (
        <div style={{ background: '#f7fafc', padding: '32px', textAlign: 'center', borderRadius: '8px' }}>
          <p>No tasks found.</p>
        </div>
      ) : (
        <div style={{ display: 'grid', gap: '16px' }}>
          {tasks.map(task => (
            <div 
              key={task.id} 
              onClick={() => navigate(`/follow-up-tasks/${task.id}`)}
              style={{ border: '1px solid #e2e8f0', padding: '16px', borderRadius: '8px', cursor: 'pointer', background: 'white', boxShadow: '0 1px 3px rgba(0,0,0,0.1)' }}
            >
              <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '8px' }}>
                <h3 style={{ fontWeight: 'bold', fontSize: '18px' }}>{task.title}</h3>
                <div style={{ display: 'flex', gap: '8px' }}>
                  {getPriorityBadge(task.priority)}
                  {getStatusBadge(task.status)}
                </div>
              </div>
              <p style={{ color: '#4a5568', fontSize: '14px', marginBottom: '16px' }}>{task.description}</p>
              
              <div style={{ display: 'flex', gap: '16px', fontSize: '12px', color: '#718096' }}>
                <span><strong>Due:</strong> {new Date(task.dueDate).toLocaleDateString()}</span>
                <span><strong>Assigned To:</strong> {task.assignedTo}</span>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { carePlanApi } from '../../core/api/carePlanApi';
import { CarePlanDocument, CarePlanStatus, Priority, GoalStatus, TaskStatus } from './types';

export const CarePlanDetails: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [plan, setPlan] = useState<CarePlanDocument | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [updating, setUpdating] = useState(false);

  // Quick states for adding nested items
  const [showAddGoal, setShowAddGoal] = useState(false);
  const [showAddTask, setShowAddTask] = useState(false);
  const { register: registerGoal, handleSubmit: handleGoalSubmit, reset: resetGoal } = useForm();
  const { register: registerTask, handleSubmit: handleTaskSubmit, reset: resetTask } = useForm();

  const fetchPlan = async () => {
    if (!id) return;
    try {
      setLoading(true);
      const data = await carePlanApi.getCarePlanById(id);
      setPlan(data);
    } catch (err) {
      setError('Failed to load care plan details.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchPlan();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [id]);

  const handleStatusUpdate = async (newStatus: CarePlanStatus) => {
    if (!id || !plan) return;
    setUpdating(true);
    setError(null);
    try {
      await carePlanApi.updateCarePlanStatus(id, {
        status: newStatus,
        reason: 'Updated via Portal',
        updatedBy: 'SystemUser'
      });
      await fetchPlan();
    } catch (err: any) {
      setError(err.response?.data?.detail || 'Failed to update status.');
    } finally {
      setUpdating(false);
    }
  };

  const onAddGoal = async (data: any) => {
    if (!id) return;
    try {
      await carePlanApi.addGoal(id, {
        description: data.description,
        targetDate: data.targetDate ? new Date(data.targetDate).toISOString() : undefined,
        priority: Number(data.priority)
      });
      setShowAddGoal(false);
      resetGoal();
      fetchPlan();
    } catch (err: any) {
      setError('Failed to add goal.');
    }
  };

  const onAddTask = async (data: any) => {
    if (!id) return;
    try {
      await carePlanApi.addTask(id, {
        taskTitle: data.taskTitle,
        taskDescription: data.taskDescription,
        dueDate: data.dueDate ? new Date(data.dueDate).toISOString() : undefined,
        priority: Number(data.priority),
        assignedTo: data.assignedTo
      });
      setShowAddTask(false);
      resetTask();
      fetchPlan();
    } catch (err: any) {
      setError('Failed to add task.');
    }
  };

  const getStatusBadge = (status: CarePlanStatus) => {
    switch (status) {
      case CarePlanStatus.Draft: return <span className="badge" style={{ background: '#e2e8f0', color: '#4a5568' }}>Draft</span>;
      case CarePlanStatus.Active: return <span className="badge" style={{ background: '#e8f7f0', color: '#167455' }}>Active</span>;
      case CarePlanStatus.OnHold: return <span className="badge" style={{ background: '#fefcbf', color: '#744210' }}>On Hold</span>;
      case CarePlanStatus.Completed: return <span className="badge" style={{ background: '#c6f6d5', color: '#22543d' }}>Completed</span>;
      case CarePlanStatus.Cancelled: return <span className="badge" style={{ background: '#fed7d7', color: '#822727' }}>Cancelled</span>;
      default: return <span className="badge">Unknown</span>;
    }
  };

  const getPriorityString = (p: Priority) => {
    switch(p) {
      case Priority.Low: return 'Low';
      case Priority.Medium: return 'Medium';
      case Priority.High: return 'High';
      case Priority.Critical: return 'Critical';
      default: return 'Unknown';
    }
  };

  const getGoalStatusString = (s: GoalStatus) => {
    switch(s) {
      case GoalStatus.NotStarted: return 'Not Started';
      case GoalStatus.InProgress: return 'In Progress';
      case GoalStatus.Achieved: return 'Achieved';
      case GoalStatus.NotAchieved: return 'Not Achieved';
      case GoalStatus.Cancelled: return 'Cancelled';
      default: return 'Unknown';
    }
  };

  const getTaskStatusString = (s: TaskStatus) => {
    switch(s) {
      case TaskStatus.Pending: return 'Pending';
      case TaskStatus.InProgress: return 'In Progress';
      case TaskStatus.Completed: return 'Completed';
      case TaskStatus.Overdue: return 'Overdue';
      case TaskStatus.Cancelled: return 'Cancelled';
      default: return 'Unknown';
    }
  };

  if (loading) return <div className="page-section"><p>Loading plan...</p></div>;
  if (!plan) return <div className="page-section"><p style={{ color: 'red' }}>Plan not found.</p></div>;

  return (
    <div className="page-section">
      <div className="section-header" style={{ marginBottom: '20px' }}>
        <div style={{ display: 'flex', alignItems: 'center', gap: '16px' }}>
          <Link to="/care-plans" style={{ textDecoration: 'none', color: '#62707d', fontWeight: 600 }}>&larr; Back</Link>
          <h2 style={{ margin: 0 }}>Care Plan Details</h2>
          {getStatusBadge(plan.status)}
        </div>
      </div>

      {error && <div style={{ color: 'red', marginBottom: '16px', padding: '12px', background: '#ffe6e6', borderRadius: '4px' }}>{error}</div>}

      <div className="module-grid" style={{ gridTemplateColumns: '2fr 1fr' }}>
        
        {/* Left Column: Document info and nested collections */}
        <div style={{ display: 'flex', flexDirection: 'column', gap: '20px' }}>
          <div className="info-panel">
            <h3>{plan.title}</h3>
            <div style={{ marginTop: '16px', display: 'flex', flexDirection: 'column', gap: '12px' }}>
              <div>
                <strong>Clinical Summary</strong>
                <p style={{ margin: '4px 0 0 0' }}>{plan.clinicalSummary}</p>
              </div>
              <div>
                <strong>Instructions</strong>
                <p style={{ margin: '4px 0 0 0' }}>{plan.instructions}</p>
              </div>
              <div style={{ display: 'flex', gap: '30px', marginTop: '12px' }}>
                <div>
                  <strong>Patient ID</strong>
                  <span style={{ display: 'block' }}>{plan.patientId}</span>
                </div>
                <div>
                  <strong>Provider ID</strong>
                  <span style={{ display: 'block' }}>{plan.providerId}</span>
                </div>
                <div>
                  <strong>Follow Up Date</strong>
                  <span style={{ display: 'block' }}>{plan.followUpDate ? new Date(plan.followUpDate).toLocaleDateString() : 'N/A'}</span>
                </div>
              </div>
            </div>
          </div>

          <div className="info-panel">
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
              <h3>Goals</h3>
              <button onClick={() => setShowAddGoal(!showAddGoal)} style={{ padding: '4px 8px', borderRadius: '4px', border: '1px solid #d9e4e7', cursor: 'pointer' }}>+ Add Goal</button>
            </div>
            
            {showAddGoal && (
              <form onSubmit={handleGoalSubmit(onAddGoal)} style={{ marginTop: '12px', padding: '12px', border: '1px dashed #ccc', borderRadius: '4px', display: 'flex', flexDirection: 'column', gap: '8px' }}>
                <input placeholder="Description" required {...registerGoal('description')} style={{ padding: '8px' }} />
                <input type="date" {...registerGoal('targetDate')} style={{ padding: '8px' }} />
                <select {...registerGoal('priority')} style={{ padding: '8px' }}>
                  <option value={Priority.Low}>Low</option>
                  <option value={Priority.Medium}>Medium</option>
                  <option value={Priority.High}>High</option>
                  <option value={Priority.Critical}>Critical</option>
                </select>
                <button type="submit" style={{ padding: '8px', background: '#21a67a', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>Save Goal</button>
              </form>
            )}

            <ul style={{ listStyle: 'none', padding: 0, marginTop: '16px', display: 'flex', flexDirection: 'column', gap: '12px' }}>
              {plan.goals && plan.goals.length > 0 ? plan.goals.map(g => (
                <li key={g.goalId} style={{ padding: '12px', border: '1px solid #d9e4e7', borderRadius: '4px', display: 'flex', justifyContent: 'space-between' }}>
                  <div>
                    <strong style={{ display: 'block' }}>{g.description}</strong>
                    <span style={{ fontSize: '0.85rem', color: '#62707d' }}>Target: {g.targetDate ? new Date(g.targetDate).toLocaleDateString() : 'N/A'}</span>
                  </div>
                  <div style={{ textAlign: 'right' }}>
                    <span className="badge" style={{ display: 'block', marginBottom: '4px' }}>{getGoalStatusString(g.status)}</span>
                    <span style={{ fontSize: '0.85rem' }}>Priority: {getPriorityString(g.priority)}</span>
                  </div>
                </li>
              )) : <li style={{ color: '#62707d' }}>No goals defined.</li>}
            </ul>
          </div>

          <div className="info-panel">
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
              <h3>Tasks</h3>
              <button onClick={() => setShowAddTask(!showAddTask)} style={{ padding: '4px 8px', borderRadius: '4px', border: '1px solid #d9e4e7', cursor: 'pointer' }}>+ Add Task</button>
            </div>
            
            {showAddTask && (
              <form onSubmit={handleTaskSubmit(onAddTask)} style={{ marginTop: '12px', padding: '12px', border: '1px dashed #ccc', borderRadius: '4px', display: 'flex', flexDirection: 'column', gap: '8px' }}>
                <input placeholder="Task Title" required {...registerTask('taskTitle')} style={{ padding: '8px' }} />
                <input placeholder="Task Description" {...registerTask('taskDescription')} style={{ padding: '8px' }} />
                <input placeholder="Assigned To (e.g. CareCoordinator)" required {...registerTask('assignedTo')} style={{ padding: '8px' }} />
                <input type="date" {...registerTask('dueDate')} style={{ padding: '8px' }} />
                <select {...registerTask('priority')} style={{ padding: '8px' }}>
                  <option value={Priority.Low}>Low</option>
                  <option value={Priority.Medium}>Medium</option>
                  <option value={Priority.High}>High</option>
                  <option value={Priority.Critical}>Critical</option>
                </select>
                <button type="submit" style={{ padding: '8px', background: '#21a67a', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>Save Task</button>
              </form>
            )}

            <ul style={{ listStyle: 'none', padding: 0, marginTop: '16px', display: 'flex', flexDirection: 'column', gap: '12px' }}>
              {plan.tasks && plan.tasks.length > 0 ? plan.tasks.map(t => (
                <li key={t.taskId} style={{ padding: '12px', border: '1px solid #d9e4e7', borderRadius: '4px', display: 'flex', justifyContent: 'space-between' }}>
                  <div>
                    <strong style={{ display: 'block' }}>{t.taskTitle}</strong>
                    <span style={{ fontSize: '0.85rem', color: '#62707d' }}>{t.taskDescription}</span>
                    <span style={{ fontSize: '0.85rem', color: '#62707d', display: 'block', marginTop: '4px' }}>Assigned to: {t.assignedTo} | Due: {t.dueDate ? new Date(t.dueDate).toLocaleDateString() : 'N/A'}</span>
                  </div>
                  <div style={{ textAlign: 'right' }}>
                    <span className="badge" style={{ display: 'block', marginBottom: '4px' }}>{getTaskStatusString(t.status)}</span>
                    <span style={{ fontSize: '0.85rem' }}>Priority: {getPriorityString(t.priority)}</span>
                  </div>
                </li>
              )) : <li style={{ color: '#62707d' }}>No tasks assigned.</li>}
            </ul>
          </div>
        </div>

        {/* Right Column: Workflow Actions */}
        <div className="info-panel" style={{ alignSelf: 'start' }}>
          <h3>Workflow Actions</h3>
          <p style={{ fontSize: '0.9rem', color: '#62707d', marginBottom: '16px' }}>Manage the care plan status transition.</p>
          
          <div style={{ display: 'flex', flexDirection: 'column', gap: '12px' }}>
            {plan.status === CarePlanStatus.Draft && (
              <>
                <button disabled={updating} onClick={() => handleStatusUpdate(CarePlanStatus.Active)} style={{ padding: '8px', background: '#e8f7f0', color: '#167455', border: '1px solid #167455', borderRadius: '4px', cursor: 'pointer', fontWeight: 600 }}>Activate Plan</button>
                <button disabled={updating} onClick={() => handleStatusUpdate(CarePlanStatus.Cancelled)} style={{ padding: '8px', background: '#fed7d7', color: '#822727', border: '1px solid #822727', borderRadius: '4px', cursor: 'pointer', fontWeight: 600 }}>Cancel Plan</button>
              </>
            )}

            {plan.status === CarePlanStatus.Active && (
              <>
                <button disabled={updating} onClick={() => handleStatusUpdate(CarePlanStatus.OnHold)} style={{ padding: '8px', background: '#fefcbf', color: '#744210', border: '1px solid #744210', borderRadius: '4px', cursor: 'pointer', fontWeight: 600 }}>Place On Hold</button>
                <button disabled={updating} onClick={() => handleStatusUpdate(CarePlanStatus.Completed)} style={{ padding: '8px', background: '#c6f6d5', color: '#22543d', border: '1px solid #22543d', borderRadius: '4px', cursor: 'pointer', fontWeight: 600 }}>Mark Completed</button>
                <button disabled={updating} onClick={() => handleStatusUpdate(CarePlanStatus.Cancelled)} style={{ padding: '8px', background: '#fed7d7', color: '#822727', border: '1px solid #822727', borderRadius: '4px', cursor: 'pointer', fontWeight: 600 }}>Cancel Plan</button>
              </>
            )}

            {plan.status === CarePlanStatus.OnHold && (
              <>
                <button disabled={updating} onClick={() => handleStatusUpdate(CarePlanStatus.Active)} style={{ padding: '8px', background: '#e8f7f0', color: '#167455', border: '1px solid #167455', borderRadius: '4px', cursor: 'pointer', fontWeight: 600 }}>Re-activate Plan</button>
                <button disabled={updating} onClick={() => handleStatusUpdate(CarePlanStatus.Cancelled)} style={{ padding: '8px', background: '#fed7d7', color: '#822727', border: '1px solid #822727', borderRadius: '4px', cursor: 'pointer', fontWeight: 600 }}>Cancel Plan</button>
              </>
            )}

            {(plan.status === CarePlanStatus.Completed || plan.status === CarePlanStatus.Cancelled) && (
              <p style={{ fontStyle: 'italic', color: '#62707d' }}>This plan has reached a final state.</p>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

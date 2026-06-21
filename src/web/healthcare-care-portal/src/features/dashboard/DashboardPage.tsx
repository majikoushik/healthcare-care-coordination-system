import React, { useEffect, useState } from 'react';
import { patientApi } from '../../core/api/patientApi';
import { providerApi } from '../../core/api/providerApi';
import { appointmentApi } from '../../core/api/appointmentApi';
import { carePlanApi } from '../../core/api/carePlanApi';
import { followUpTaskApi } from '../../core/api/followUpTaskApi';
import { Link } from 'react-router-dom';

const workstreams = [
  ["Patient coordination", "Registration and consent readiness", "SQL"],
  ["Appointments", "Scheduling lifecycle boundary", "SQL"],
  ["Care plans", "Goals, instructions, and follow-ups", "Cosmos"],
  ["Clinical insights", "Mock AI provider with Azure AI Language readiness", "AI-ready"],
  ["Follow-up Tasks", "Due dates, priorities, and workflow execution", "Cosmos"],
  ["Observability", "Correlation IDs, health checks, structured logs", "Ops"]
] as const;

export function DashboardPage() {
  const [stats, setStats] = useState({
    patients: 0,
    providers: 0,
    appointments: 0,
    carePlans: 0,
    overdueTasks: 0,
    dueTodayTasks: 0
  });

  useEffect(() => {
    async function fetchData() {
      try {
        const [patients, providers, appointments, carePlans, overdue, dueToday] = await Promise.all([
          patientApi.getPatients(),
          providerApi.getProviders(),
          appointmentApi.getAppointments(),
          carePlanApi.getCarePlans(),
          followUpTaskApi.getOverdue(),
          followUpTaskApi.getDueToday()
        ]);
        
        setStats({ 
          patients: patients.length, 
          providers: providers.length, 
          appointments: appointments.length, 
          carePlans: carePlans.length, 
          overdueTasks: overdue.length, 
          dueTodayTasks: dueToday.length 
        });
      } catch (err) {
        console.error("Failed to load dashboard stats", err);
      }
    }
    fetchData();
  }, []);

  return (
    <section className="dashboard">
      <div className="dashboard-intro">
        <div>
          <p className="eyebrow">Portfolio architecture foundation</p>
          <h2>Cloud-native healthcare coordination, ready for MVP epics.</h2>
          <p>
            The foundation separates transactional healthcare master data from flexible coordination documents,
            prepares clinical AI through a safe mock provider, and keeps privacy constraints visible from day one.
          </p>
        </div>
        <div className="readiness-card">
          <span>Local default</span>
          <strong>AI_PROVIDER=Mock</strong>
          <small>Azure credentials are not required for local development or CI.</small>
          
          <div style={{ marginTop: '16px', display: 'flex', gap: '8px', flexDirection: 'column' }}>
            <Link to="/follow-up-tasks" style={{ textDecoration: 'none', display: 'block' }}>
              <div style={{ background: stats.overdueTasks > 0 ? '#fed7d7' : '#f7fafc', padding: '12px', borderRadius: '4px', border: '1px solid #e2e8f0', display: 'flex', justifyContent: 'space-between' }}>
                <span style={{ color: stats.overdueTasks > 0 ? '#c53030' : '#4a5568', fontWeight: 'bold' }}>Overdue Tasks</span>
                <span style={{ fontWeight: 'bold' }}>{stats.overdueTasks}</span>
              </div>
            </Link>
            
            <Link to="/follow-up-tasks" style={{ textDecoration: 'none', display: 'block' }}>
              <div style={{ background: stats.dueTodayTasks > 0 ? '#feebc8' : '#f7fafc', padding: '12px', borderRadius: '4px', border: '1px solid #e2e8f0', display: 'flex', justifyContent: 'space-between' }}>
                <span style={{ color: stats.dueTodayTasks > 0 ? '#dd6b20' : '#4a5568', fontWeight: 'bold' }}>Tasks Due Today</span>
                <span style={{ fontWeight: 'bold' }}>{stats.dueTodayTasks}</span>
              </div>
            </Link>
          </div>
        </div>
      </div>
      <div className="workstream-grid">
        {workstreams.map(([title, description, tag]) => (
          <article className="workstream-card" key={title}>
            <span className="badge">{tag}</span>
            <h3>{title}</h3>
            <p>{description}</p>
          </article>
        ))}
      </div>
    </section>
  );
}

import React, { useEffect, useState } from 'react';
import { patientApi } from '../../core/api/patientApi';
import { providerApi } from '../../core/api/providerApi';
import { appointmentApi } from '../../core/api/appointmentApi';
import { carePlanApi } from '../../core/api/carePlanApi';
import { followUpTaskApi } from '../../core/api/followUpTaskApi';
import { notificationApi } from '../../core/api/notificationApi';
import { Link } from 'react-router-dom';

const workstreams = [
  ["Patient coordination", "Registration, contact profile, and consent readiness", "SQL"],
  ["Appointments", "Scheduling lifecycle and status transition boundary", "SQL"],
  ["Care plans", "Goals, instructions, embedded tasks, and follow-ups", "Cosmos"],
  ["Clinical insights", "Mock AI provider with Azure AI Language readiness", "AI-ready"],
  ["Follow-up Tasks", "Due dates, priorities, and workflow execution", "Cosmos"],
  ["Audit and observability", "Traceability, correlation IDs, health checks, structured logs", "Ops"]
] as const;

const portfolioMetrics = [
  ["Synthetic patients", "patients"],
  ["Active providers", "providers"],
  ["Appointments", "appointments"],
  ["Active care plans", "carePlans"]
] as const;

export function DashboardPage() {
  const [stats, setStats] = useState({
    patients: 0,
    providers: 0,
    appointments: 0,
    carePlans: 0,
    overdueTasks: 0,
    dueTodayTasks: 0,
    queuedNotifications: 0
  });

  useEffect(() => {
    async function fetchData() {
      try {
        const [patients, providers, appointments, carePlans, overdue, dueToday, notifications] = await Promise.all([
          patientApi.getPatients(),
          providerApi.getProviders(),
          appointmentApi.getAppointments(),
          carePlanApi.getCarePlans(),
          followUpTaskApi.getOverdue(),
          followUpTaskApi.getDueToday(),
          notificationApi.getNotifications()
        ]);
        
        const queued = notifications.filter(n => n.status === 1 || n.status === 0).length;

        setStats({ 
          patients: patients.length, 
          providers: providers.length, 
          appointments: appointments.length, 
          carePlans: carePlans.length, 
          overdueTasks: overdue.length, 
          dueTodayTasks: dueToday.length,
          queuedNotifications: queued
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
          <p className="eyebrow">Healthcare operations portfolio demo</p>
          <h2>Cloud-native care coordination across patient, provider, appointment, care plan, insight, notification, and audit workflows.</h2>
          <p>
            The portal separates transactional healthcare master data from flexible coordination documents,
            prepares clinical AI through a safe mock provider, and keeps privacy, responsible AI, and traceability constraints visible.
          </p>
          <div className="metric-grid">
            {portfolioMetrics.map(([label, key]) => (
              <Link className="metric-card" key={label} to={
                key === "patients" ? "/patients" :
                key === "providers" ? "/providers" :
                key === "appointments" ? "/appointments" : "/care-plans"
              }>
                <span>{label}</span>
                <strong>{stats[key]}</strong>
              </Link>
            ))}
          </div>
        </div>
        <div className="readiness-card">
          <span>Local default</span>
          <strong>AI_PROVIDER=Mock</strong>
          <small>Azure credentials are not required for local development or CI.</small>
          
          <div className="operational-list">
            <Link to="/follow-up-tasks" className={stats.overdueTasks > 0 ? "ops-item alert" : "ops-item"}>
              <span>Overdue Tasks</span>
              <strong>{stats.overdueTasks}</strong>
            </Link>
            
            <Link to="/follow-up-tasks" className={stats.dueTodayTasks > 0 ? "ops-item warning" : "ops-item"}>
              <span>Tasks Due Today</span>
              <strong>{stats.dueTodayTasks}</strong>
            </Link>

            <Link to="/notifications" className={stats.queuedNotifications > 0 ? "ops-item info" : "ops-item"}>
              <span>Pending Notifications</span>
              <strong>{stats.queuedNotifications}</strong>
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

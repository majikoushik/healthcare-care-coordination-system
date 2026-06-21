import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { patientApi } from '../../core/api/patientApi';
import { Patient, Gender, ConsentStatus } from './types';
import { PrivacyNotice } from '../../shared/ui/PrivacyNotice';

export const PatientDetails: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [patient, setPatient] = useState<Patient | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchPatient = async () => {
      if (!id) return;
      try {
        const data = await patientApi.getPatientById(id);
        setPatient(data);
      } catch (err) {
        setError('Failed to load patient details.');
      } finally {
        setLoading(false);
      }
    };
    fetchPatient();
  }, [id]);

  const getGenderString = (g: Gender) => {
    switch (g) {
      case Gender.Male: return 'Male';
      case Gender.Female: return 'Female';
      case Gender.Other: return 'Other';
      default: return 'Prefer not to say';
    }
  };

  const getConsentBadge = (status: ConsentStatus) => {
    switch (status) {
      case ConsentStatus.Provided:
        return <span className="badge" style={{ background: '#e8f7f0', color: '#167455' }}>Provided</span>;
      case ConsentStatus.Withdrawn:
        return <span className="badge" style={{ background: '#fce8e8', color: '#c53030' }}>Withdrawn</span>;
      default:
        return <span className="badge" style={{ background: '#edf2f7', color: '#4a5568' }}>Not Provided</span>;
    }
  };

  if (loading) return <div className="page-section"><p>Loading patient...</p></div>;
  if (error || !patient) return <div className="page-section"><p style={{ color: 'red' }}>{error || 'Patient not found'}</p></div>;

  return (
    <div className="page-section">
      <PrivacyNotice />
      <div className="section-header" style={{ marginBottom: '20px' }}>
        <div style={{ display: 'flex', alignItems: 'center', gap: '16px' }}>
          <Link to="/patients" style={{ textDecoration: 'none', color: '#62707d', fontWeight: 600 }}>&larr; Back to Patients</Link>
          <h2 style={{ margin: 0 }}>{patient.fullName}</h2>
          {getConsentBadge(patient.consentStatus)}
        </div>
      </div>

      <div className="module-grid" style={{ gridTemplateColumns: '1fr 1fr' }}>
        <div className="info-panel">
          <h3>Profile Information</h3>
          <div style={{ marginTop: '16px', display: 'flex', flexDirection: 'column', gap: '12px' }}>
            <div>
              <strong>Patient ID</strong>
              <span>{patient.id}</span>
            </div>
            <div>
              <strong>Date of Birth</strong>
              <span>{new Date(patient.dateOfBirth).toLocaleDateString()}</span>
            </div>
            <div>
              <strong>Gender</strong>
              <span>{getGenderString(patient.gender)}</span>
            </div>
            <div>
              <strong>Registered On</strong>
              <span>{new Date(patient.createdAt).toLocaleString()}</span>
            </div>
          </div>
        </div>

        <div className="info-panel">
          <h3>Contact Details</h3>
          <div style={{ marginTop: '16px', display: 'flex', flexDirection: 'column', gap: '12px' }}>
            <div>
              <strong>Email</strong>
              <span>{patient.email}</span>
            </div>
            <div>
              <strong>Mobile Number</strong>
              <span>{patient.mobileNumber}</span>
            </div>
            <div>
              <strong>Address</strong>
              <span>{patient.address}</span>
            </div>
          </div>
        </div>

        <div className="info-panel" style={{ gridColumn: '1 / -1' }}>
          <h3>Emergency Contact</h3>
          <div style={{ marginTop: '16px', display: 'flex', gap: '32px' }}>
            <div>
              <strong>Name</strong>
              <span>{patient.emergencyContactName}</span>
            </div>
            <div>
              <strong>Number</strong>
              <span>{patient.emergencyContactNumber}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

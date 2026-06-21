import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { patientApi } from '../../core/api/patientApi';
import { Patient, Gender, ConsentStatus } from './types';

export const PatientList: React.FC = () => {
  const [patients, setPatients] = useState<Patient[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchPatients = async () => {
      try {
        const data = await patientApi.getPatients();
        setPatients(data);
      } catch (err) {
        setError('Failed to load patients.');
      } finally {
        setLoading(false);
      }
    };
    fetchPatients();
  }, []);

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

  return (
    <div className="page-section">
      <div className="section-header">
        <h2>Patients</h2>
        <Link to="/patients/new" style={{ background: '#21a67a', color: 'white', padding: '8px 16px', borderRadius: '4px', textDecoration: 'none', fontWeight: 600 }}>
          + Register Patient
        </Link>
      </div>

      <div className="dashboard-intro" style={{ marginTop: '20px', display: 'block' }}>
        {loading ? (
          <p>Loading patients...</p>
        ) : error ? (
          <p style={{ color: 'red' }}>{error}</p>
        ) : patients.length === 0 ? (
          <p>No patients found. Please register a new patient.</p>
        ) : (
          <div style={{ overflowX: 'auto' }}>
            <table style={{ width: '100%', borderCollapse: 'collapse', textAlign: 'left' }}>
              <thead>
                <tr style={{ borderBottom: '2px solid #d9e4e7' }}>
                  <th style={{ padding: '12px 8px' }}>Name</th>
                  <th style={{ padding: '12px 8px' }}>DOB</th>
                  <th style={{ padding: '12px 8px' }}>Gender</th>
                  <th style={{ padding: '12px 8px' }}>Consent</th>
                  <th style={{ padding: '12px 8px' }}>Action</th>
                </tr>
              </thead>
              <tbody>
                {patients.map(p => (
                  <tr key={p.id} style={{ borderBottom: '1px solid #d9e4e7' }}>
                    <td style={{ padding: '12px 8px', fontWeight: 600 }}>{p.fullName}</td>
                    <td style={{ padding: '12px 8px' }}>{new Date(p.dateOfBirth).toLocaleDateString()}</td>
                    <td style={{ padding: '12px 8px' }}>{getGenderString(p.gender)}</td>
                    <td style={{ padding: '12px 8px' }}>{getConsentBadge(p.consentStatus)}</td>
                    <td style={{ padding: '12px 8px' }}>
                      <Link to={`/patients/${p.id}`} style={{ color: '#21a67a', textDecoration: 'none', fontWeight: 600 }}>View</Link>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
};

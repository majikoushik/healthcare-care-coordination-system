import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { providerApi } from '../../core/api/providerApi';
import { Provider, Specialty, AvailabilityStatus } from './types';

export const ProviderList: React.FC = () => {
  const [providers, setProviders] = useState<Provider[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchProviders = async () => {
      try {
        const data = await providerApi.getProviders();
        setProviders(data);
      } catch (err) {
        setError('Failed to load providers.');
      } finally {
        setLoading(false);
      }
    };
    fetchProviders();
  }, []);

  const getSpecialtyString = (s: Specialty) => {
    switch (s) {
      case Specialty.GeneralMedicine: return 'General Medicine';
      case Specialty.Cardiology: return 'Cardiology';
      case Specialty.Endocrinology: return 'Endocrinology';
      case Specialty.Orthopedics: return 'Orthopedics';
      case Specialty.Pediatrics: return 'Pediatrics';
      case Specialty.Neurology: return 'Neurology';
      case Specialty.Dermatology: return 'Dermatology';
      default: return 'Unknown';
    }
  };

  const getAvailabilityBadge = (status: AvailabilityStatus) => {
    switch (status) {
      case AvailabilityStatus.Available:
        return <span className="badge" style={{ background: '#e8f7f0', color: '#167455' }}>Available</span>;
      case AvailabilityStatus.Busy:
        return <span className="badge" style={{ background: '#feebc8', color: '#c05621' }}>Busy</span>;
      case AvailabilityStatus.OnLeave:
        return <span className="badge" style={{ background: '#e2e8f0', color: '#4a5568' }}>On Leave</span>;
      case AvailabilityStatus.Inactive:
        return <span className="badge" style={{ background: '#fce8e8', color: '#c53030' }}>Inactive</span>;
      default:
        return <span className="badge">Unknown</span>;
    }
  };

  return (
    <div className="page-section">
      <div className="section-header">
        <h2>Providers</h2>
        <Link to="/providers/new" style={{ background: '#21a67a', color: 'white', padding: '8px 16px', borderRadius: '4px', textDecoration: 'none', fontWeight: 600 }}>
          + Register Provider
        </Link>
      </div>

      <div className="dashboard-intro" style={{ marginTop: '20px', display: 'block' }}>
        {loading ? (
          <p>Loading providers...</p>
        ) : error ? (
          <p style={{ color: 'red' }}>{error}</p>
        ) : providers.length === 0 ? (
          <p>No providers found. Please register a new provider.</p>
        ) : (
          <div style={{ overflowX: 'auto' }}>
            <table style={{ width: '100%', borderCollapse: 'collapse', textAlign: 'left' }}>
              <thead>
                <tr style={{ borderBottom: '2px solid #d9e4e7' }}>
                  <th style={{ padding: '12px 8px' }}>Name</th>
                  <th style={{ padding: '12px 8px' }}>Specialty</th>
                  <th style={{ padding: '12px 8px' }}>Department</th>
                  <th style={{ padding: '12px 8px' }}>Availability</th>
                  <th style={{ padding: '12px 8px' }}>Action</th>
                </tr>
              </thead>
              <tbody>
                {providers.map(p => (
                  <tr key={p.id} style={{ borderBottom: '1px solid #d9e4e7' }}>
                    <td style={{ padding: '12px 8px', fontWeight: 600 }}>{p.fullName}</td>
                    <td style={{ padding: '12px 8px' }}>{getSpecialtyString(p.specialty)}</td>
                    <td style={{ padding: '12px 8px' }}>{p.department}</td>
                    <td style={{ padding: '12px 8px' }}>{getAvailabilityBadge(p.availabilityStatus)}</td>
                    <td style={{ padding: '12px 8px' }}>
                      <Link to={`/providers/${p.id}`} style={{ color: '#21a67a', textDecoration: 'none', fontWeight: 600 }}>View</Link>
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

import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { providerApi } from '../../core/api/providerApi';
import { Provider, Specialty, AvailabilityStatus } from './types';

export const ProviderDetails: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [provider, setProvider] = useState<Provider | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchProvider = async () => {
      if (!id) return;
      try {
        const data = await providerApi.getProviderById(id);
        setProvider(data);
      } catch (err) {
        setError('Failed to load provider details.');
      } finally {
        setLoading(false);
      }
    };
    fetchProvider();
  }, [id]);

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

  if (loading) return <div className="page-section"><p>Loading provider...</p></div>;
  if (error || !provider) return <div className="page-section"><p style={{ color: 'red' }}>{error || 'Provider not found'}</p></div>;

  return (
    <div className="page-section">
      <div className="section-header" style={{ marginBottom: '20px' }}>
        <div style={{ display: 'flex', alignItems: 'center', gap: '16px' }}>
          <Link to="/providers" style={{ textDecoration: 'none', color: '#62707d', fontWeight: 600 }}>&larr; Back to Providers</Link>
          <h2 style={{ margin: 0 }}>{provider.fullName}</h2>
          {getAvailabilityBadge(provider.availabilityStatus)}
        </div>
      </div>

      <div className="module-grid" style={{ gridTemplateColumns: '1fr 1fr' }}>
        <div className="info-panel">
          <h3>Professional Profile</h3>
          <div style={{ marginTop: '16px', display: 'flex', flexDirection: 'column', gap: '12px' }}>
            <div>
              <strong>Provider ID</strong>
              <span>{provider.id}</span>
            </div>
            <div>
              <strong>Specialty</strong>
              <span>{getSpecialtyString(provider.specialty)}</span>
            </div>
            <div>
              <strong>Department</strong>
              <span>{provider.department}</span>
            </div>
            <div>
              <strong>Registered On</strong>
              <span>{new Date(provider.createdAt).toLocaleString()}</span>
            </div>
          </div>
        </div>

        <div className="info-panel">
          <h3>Contact Details</h3>
          <div style={{ marginTop: '16px', display: 'flex', flexDirection: 'column', gap: '12px' }}>
            <div>
              <strong>Email</strong>
              <span>{provider.email}</span>
            </div>
            <div>
              <strong>Mobile Number</strong>
              <span>{provider.mobileNumber}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

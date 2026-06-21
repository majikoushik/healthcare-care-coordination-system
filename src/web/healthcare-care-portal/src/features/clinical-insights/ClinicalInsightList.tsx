import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { clinicalInsightApi } from '../../core/api/clinicalInsightApi';
import { ClinicalNoteInsight, HumanReviewStatus } from './types';

export const ClinicalInsightList: React.FC = () => {
  const [insights, setInsights] = useState<ClinicalNoteInsight[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchInsights = async () => {
      try {
        const data = await clinicalInsightApi.getInsights();
        setInsights(data);
      } catch (err) {
        setError('Failed to load clinical insights.');
      } finally {
        setLoading(false);
      }
    };
    fetchInsights();
  }, []);

  const getReviewStatusBadge = (status: HumanReviewStatus) => {
    switch (status) {
      case HumanReviewStatus.PendingReview:
        return <span className="badge" style={{ background: '#fefcbf', color: '#744210' }}>Pending Review</span>;
      case HumanReviewStatus.Reviewed:
        return <span className="badge" style={{ background: '#e2e8f0', color: '#4a5568' }}>Reviewed</span>;
      case HumanReviewStatus.Approved:
        return <span className="badge" style={{ background: '#c6f6d5', color: '#22543d' }}>Approved</span>;
      case HumanReviewStatus.Rejected:
        return <span className="badge" style={{ background: '#fed7d7', color: '#822727' }}>Rejected</span>;
      case HumanReviewStatus.RequiresClarification:
        return <span className="badge" style={{ background: '#feebc8', color: '#9c4221' }}>Requires Clarification</span>;
      default:
        return <span className="badge">Unknown</span>;
    }
  };

  return (
    <div className="page-section">
      <div className="section-header">
        <h2>Clinical Insights</h2>
        <Link to="/clinical-insights/new" style={{ background: '#21a67a', color: 'white', padding: '8px 16px', borderRadius: '4px', textDecoration: 'none', fontWeight: 600 }}>
          + Analyze Note
        </Link>
      </div>

      <div className="dashboard-intro" style={{ marginTop: '20px', display: 'block' }}>
        {loading ? (
          <p>Loading insights...</p>
        ) : error ? (
          <p style={{ color: 'red' }}>{error}</p>
        ) : insights.length === 0 ? (
          <p>No insights found.</p>
        ) : (
          <div style={{ overflowX: 'auto' }}>
            <table style={{ width: '100%', borderCollapse: 'collapse', textAlign: 'left' }}>
              <thead>
                <tr style={{ borderBottom: '2px solid #d9e4e7' }}>
                  <th style={{ padding: '12px 8px' }}>Created</th>
                  <th style={{ padding: '12px 8px' }}>Patient ID</th>
                  <th style={{ padding: '12px 8px' }}>Provider Name</th>
                  <th style={{ padding: '12px 8px' }}>Entities Found</th>
                  <th style={{ padding: '12px 8px' }}>Review Status</th>
                  <th style={{ padding: '12px 8px' }}>Action</th>
                </tr>
              </thead>
              <tbody>
                {insights.map(i => (
                  <tr key={i.id} style={{ borderBottom: '1px solid #d9e4e7' }}>
                    <td style={{ padding: '12px 8px' }}>{new Date(i.createdAt).toLocaleDateString()}</td>
                    <td style={{ padding: '12px 8px' }}>{i.patientId}</td>
                    <td style={{ padding: '12px 8px' }}>{i.aiProviderName}</td>
                    <td style={{ padding: '12px 8px' }}>{i.extractedEntities?.length || 0}</td>
                    <td style={{ padding: '12px 8px' }}>{getReviewStatusBadge(i.humanReviewStatus)}</td>
                    <td style={{ padding: '12px 8px' }}>
                      <Link to={`/clinical-insights/${i.id}`} style={{ color: '#21a67a', textDecoration: 'none', fontWeight: 600 }}>View</Link>
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

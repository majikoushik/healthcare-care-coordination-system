import React, { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { auditApi } from '../../core/api/auditApi';
import { Card, CardHeader, CardTitle, CardContent } from '../../shared/ui/Card';
import { Loader2, Activity, ShieldAlert, CheckCircle, XCircle } from 'lucide-react';
import { format } from 'date-fns';

export function AuditList() {
  const [selectedEventId, setSelectedEventId] = useState<string | null>(null);

  const { data, isLoading, error } = useQuery({
    queryKey: ['audit-events'],
    queryFn: auditApi.getAuditEvents,
  });

  if (isLoading) {
    return (
      <div className="flex h-64 items-center justify-center">
        <Loader2 className="h-8 w-8 animate-spin text-blue-500" />
      </div>
    );
  }

  if (error) {
    return (
      <div className="rounded-md bg-red-50 p-4">
        <h3 className="text-sm font-medium text-red-800">Error loading audit events</h3>
      </div>
    );
  }

  const events = data?.data || [];

  const getSeverityColor = (severity: string) => {
    switch (severity.toLowerCase()) {
      case 'info': return 'bg-blue-100 text-blue-800';
      case 'warning': return 'bg-yellow-100 text-yellow-800';
      case 'error': return 'bg-orange-100 text-orange-800';
      case 'critical': return 'bg-red-100 text-red-800';
      default: return 'bg-gray-100 text-gray-800';
    }
  };

  const getOutcomeIcon = (outcome: string) => {
    if (outcome.toLowerCase() === 'success') {
      return <CheckCircle className="h-4 w-4 text-green-500" />;
    }
    return <XCircle className="h-4 w-4 text-red-500" />;
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold tracking-tight text-gray-900">Audit Logs</h1>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Master List */}
        <div className="lg:col-span-2 space-y-4">
          {events.length === 0 ? (
            <Card>
              <CardContent className="flex flex-col items-center justify-center py-12 text-center">
                <ShieldAlert className="h-12 w-12 text-gray-400 mb-4" />
                <h3 className="text-lg font-medium text-gray-900">No audit events found</h3>
                <p className="mt-1 text-sm text-gray-500">System actions will appear here.</p>
              </CardContent>
            </Card>
          ) : (
            <div className="overflow-hidden bg-white shadow sm:rounded-md">
              <ul className="divide-y divide-gray-200">
                {events.map((event) => (
                  <li key={event.id}>
                    <button
                      onClick={() => setSelectedEventId(event.id)}
                      className={`block w-full text-left hover:bg-gray-50 ${selectedEventId === event.id ? 'bg-blue-50' : ''}`}
                    >
                      <div className="px-4 py-4 sm:px-6">
                        <div className="flex items-center justify-between">
                          <div className="flex items-center space-x-3">
                            {getOutcomeIcon(event.outcome)}
                            <p className="truncate text-sm font-medium text-blue-600">{event.action}</p>
                            <span className={`inline-flex items-center rounded-full px-2.5 py-0.5 text-xs font-medium ${getSeverityColor(event.severity)}`}>
                              {event.severity}
                            </span>
                          </div>
                          <div className="ml-2 flex flex-shrink-0">
                            <p className="text-sm text-gray-500">
                              {format(new Date(event.createdAt), 'MMM d, yyyy HH:mm:ss')}
                            </p>
                          </div>
                        </div>
                        <div className="mt-2 sm:flex sm:justify-between">
                          <div className="sm:flex">
                            <p className="flex items-center text-sm text-gray-500">
                              <Activity className="mr-1.5 h-4 w-4 flex-shrink-0 text-gray-400" />
                              {event.summary}
                            </p>
                          </div>
                          <div className="mt-2 flex items-center text-sm text-gray-500 sm:mt-0">
                            <span className="truncate">Actor: {event.actorType} ({event.sourceService})</span>
                          </div>
                        </div>
                      </div>
                    </button>
                  </li>
                ))}
              </ul>
            </div>
          )}
        </div>

        {/* Detail Panel */}
        <div className="lg:col-span-1">
          {selectedEventId ? (
            <AuditDetailPanel eventId={selectedEventId} />
          ) : (
            <Card>
              <CardContent className="py-12 text-center text-gray-500 text-sm">
                Select an audit event to view safe metadata details.
              </CardContent>
            </Card>
          )}
        </div>
      </div>
    </div>
  );
}

function AuditDetailPanel({ eventId }: { eventId: string }) {
  const { data, isLoading, error } = useQuery({
    queryKey: ['audit-event', eventId],
    queryFn: () => auditApi.getAuditEventById(eventId),
  });

  if (isLoading) {
    return <Card><CardContent className="py-12 flex justify-center"><Loader2 className="h-6 w-6 animate-spin text-gray-400" /></CardContent></Card>;
  }

  if (error || !data) {
    return <Card><CardContent className="py-6 text-red-500 text-sm">Failed to load details.</CardContent></Card>;
  }

  const event = data.data;

  return (
    <Card className="sticky top-6">
      <CardHeader>
        <CardTitle className="text-base font-semibold">Audit Detail</CardTitle>
      </CardHeader>
      <CardContent className="space-y-4">
        <div>
          <h4 className="text-xs font-medium text-gray-500 uppercase tracking-wider">Correlation ID</h4>
          <p className="mt-1 text-sm text-gray-900 font-mono">{event.correlationId}</p>
        </div>
        
        <div className="grid grid-cols-2 gap-4">
          <div>
            <h4 className="text-xs font-medium text-gray-500 uppercase tracking-wider">Entity Type</h4>
            <p className="mt-1 text-sm text-gray-900">{event.entityType}</p>
          </div>
          <div>
            <h4 className="text-xs font-medium text-gray-500 uppercase tracking-wider">Entity ID</h4>
            <p className="mt-1 text-sm text-gray-900 font-mono truncate" title={event.entityId}>{event.entityId}</p>
          </div>
        </div>

        <div>
          <h4 className="text-xs font-medium text-gray-500 uppercase tracking-wider">Action / Outcome</h4>
          <p className="mt-1 text-sm text-gray-900">{event.action} - {event.outcome}</p>
        </div>

        <div>
          <h4 className="text-xs font-medium text-gray-500 uppercase tracking-wider">Safe Metadata</h4>
          <div className="mt-1 rounded-md bg-gray-50 p-3 overflow-x-auto">
            <pre className="text-xs text-gray-700 font-mono">
              {event.metadata ? JSON.stringify(event.metadata, null, 2) : 'No metadata'}
            </pre>
          </div>
        </div>
      </CardContent>
    </Card>
  );
}

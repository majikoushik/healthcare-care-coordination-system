import React, { Component, ErrorInfo, ReactNode } from "react";

interface Props {
  children?: ReactNode;
}

interface State {
  hasError: boolean;
  error: Error | null;
  correlationId: string | null;
}

export class ErrorBoundary extends Component<Props, State> {
  public state: State = {
    hasError: false,
    error: null,
    correlationId: null
  };

  public static getDerivedStateFromError(error: Error): State {
    return { hasError: true, error, correlationId: null };
  }

  public componentDidCatch(error: Error, errorInfo: ErrorInfo) {
    console.error("Uncaught error:", error, errorInfo);
    // In a real app, send to Application Insights / Sentry here
  }

  public render() {
    if (this.state.hasError) {
      return (
        <div style={{ padding: '40px', textAlign: 'center', fontFamily: 'system-ui' }}>
          <h1 style={{ color: '#c53030' }}>Something went wrong.</h1>
          <p>We apologize for the inconvenience. Our telemetry systems have logged this issue.</p>
          <div style={{ background: '#f7fafc', padding: '20px', borderRadius: '8px', display: 'inline-block', marginTop: '20px', textAlign: 'left' }}>
            <p><strong>Error:</strong> {this.state.error?.message}</p>
            <p style={{ color: '#718096', fontSize: '0.875rem' }}>If reporting this issue, please mention that it occurred on the Care Coordination Portal.</p>
          </div>
          <br />
          <button 
            onClick={() => window.location.href = '/'}
            style={{ marginTop: '20px', background: '#1a365d', color: 'white', border: 'none', padding: '10px 20px', borderRadius: '4px', cursor: 'pointer' }}
          >
            Return to Dashboard
          </button>
        </div>
      );
    }

    return this.props.children;
  }
}

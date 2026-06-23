# Clinical AI Architecture

This document describes the design patterns used to integrate AI text analytics into the Healthcare Care Coordination System.

## Responsible AI by Design

The platform employs a **Human-Review-First** architecture.
- **No Direct Action**: AI output (e.g., extracted risk factors or conditions) cannot automatically trigger patient workflows (like creating a Care Plan) without a Human Reviewer explicitly approving the `ClinicalNoteInsight` document.
- **Synthetic Data Only**: Real patient health records are never ingested.
- **Not a Diagnostic Tool**: The UI clearly disclaims that the AI output is assistive only and requires clinical review.

## IClinicalTextAnalyzer Abstraction

To avoid tight coupling to Azure SDKs during local development, we define `IClinicalTextAnalyzer`.
- `MockClinicalTextAnalyzer`: A deterministic, keyword-matching implementation used natively for local development. It runs purely in-memory and requires no API keys.
- `AzureTextAnalyticsForHealthProvider`: A readiness placeholder demonstrating the intended Azure architecture. It simulates the expected Text Analytics for Health output shape locally without requiring real Azure credentials. (Future production implementation will swap this simulation to call the true `Azure.AI.TextAnalytics` cognitive service.)

## Data Persistence

Clinical Insights are massive, unstructured JSON documents that naturally fit a NoSQL store. We persist these in Azure Cosmos DB with the partition key `/patientId`.
We deliberately **do not log** the raw clinical note text to application telemetry (`ILogger`), as it represents sensitive data. Only tracing correlation IDs and safe metadata are logged.

import React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { ErrorBoundary } from './ErrorBoundary';
import { ApiError } from '../../autoapi/base';

export default {
    title: 'Components/ErrorBoundary',
    component: ErrorBoundary,
    excludeStories: /.*Data$/,
};



export const Default = () =>
{
    const apiError = { traceId: "CB66DE66-1F6B-6666-9FA8-83E22135C715", detail: "You have done something terrible, shame will never give you the freedom you're looking for." } as ApiError;

    return (
        <ErrorBoundary error={apiError}  >
        <h1>some lorep ipsum text</h1></ErrorBoundary>
    );
};

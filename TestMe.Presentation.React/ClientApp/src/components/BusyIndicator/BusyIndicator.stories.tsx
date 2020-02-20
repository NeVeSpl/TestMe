import React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { BusyIndicator } from './BusyIndicator';

export default {
    title: 'Components/BusyIndicator',
    component: BusyIndicator,
    excludeStories: /.*Data$/,
};



export const Default = () =>
{
   

    return (
        <BusyIndicator isBusy={true} >{() => { }}</BusyIndicator>
    );
};

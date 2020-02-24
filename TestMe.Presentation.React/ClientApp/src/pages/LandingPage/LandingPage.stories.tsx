import React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { LandingPage } from './LandingPage';

export default {
    title: 'LandingPage/LandingPage',
    component: LandingPage,
};



export const Default = () => (
    <LandingPage {...({} as RouteComponentProps)}  />       
);

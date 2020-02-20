import React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { Page404 } from './Page404';

export default {
    title: 'Page404',
    component: Page404,
};



export const Default = () => (
    <Page404 {...({} as RouteComponentProps)}/>       
);

import React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { Layout } from './Layout';
import { MemoryRouter } from 'react-router'


export default {
    title: 'Components/Layout',
    component: Layout,
    excludeStories: /.*Data$/,
};



export const Default = () =>
{
   
    return (
        <MemoryRouter>
           <Layout  ><h1>some lorep ipsum text</h1></Layout>
        </MemoryRouter>
    );
};

import React from 'react';
import { Prompt } from './Prompt';
import { action } from '@storybook/addon-actions';


export default {
    title: 'Components/Prompt',
    component: Prompt,
    excludeStories: /.*Data$/,
};

export const actionsData = {
    onCancel: action('onCancel'),
    onOk: action('onOk'),
   
};

export const Default = () =>
{
   
    return (

        <Prompt message="Msg" title="title" {...actionsData} />
  
    );
};

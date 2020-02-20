import React from 'react';
import { Window } from './Window';
import { action } from '@storybook/addon-actions';


export default {
    title: 'Components/Window',
    component: Window,
    excludeStories: /.*Data$/,
};

export const actionsData = {
    onCancel: action('onCancel'),
    onOk: action('onOk'),
   
};

export const Default = () =>
{   
    return (
        <Window title="title" {...actionsData} ><div>children</div></Window>  
    );
};

export const Disabled = () => {
    return (
        <Window title="title" isEnabled={false} {...actionsData} ><div>children</div></Window>
    );
};

import { Pagination } from './Pagination';
import { action } from '@storybook/addon-actions';
import { withKnobs, text, boolean, number } from "@storybook/addon-knobs";
import * as React from 'react';


export default {
    title: 'Components/Pagination',
    component: Pagination,
    excludeStories: /.*Data$/,
    decorators: [withKnobs]
};

export const actionsData = {
    onShiftLeft: action('onShiftLeft'),
    onShiftRight: action('onShiftRight'),
};


export const Default = () =>
{

    return (
        <Pagination {...actionsData} canShiftRight={boolean("canShiftRight", true)} />       
    );
};

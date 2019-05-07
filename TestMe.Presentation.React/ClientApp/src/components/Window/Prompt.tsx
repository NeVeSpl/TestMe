import * as React from 'react';
import { WindowTemplate } from './WindowTemplate';

interface PromptProps
{
    message: string;
    onCancel: () => void;
    onOk: () => void;
    title?: string;
    level?: number;
}


export function Prompt(props: PromptProps)
{
    const footer =
        <>
            <button className="btn btn-primary mr-1" onClick={props.onCancel} >Cancel</button>
            <button className="btn btn-primary" onClick={props.onOk} >Ok</button>
        </>
        ;            

    return (
        <WindowTemplate footer={footer} header={<h5>{props.title}</h5>} level={props.level!}>
            <p>{props.message}</p>
        </WindowTemplate>
    );    
}
Prompt.defaultProps =
{
    title: "",
    level : 0
}

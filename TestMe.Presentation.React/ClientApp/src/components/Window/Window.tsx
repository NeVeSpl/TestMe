import * as React from 'react';
import styles from './Window.module.css';
import { ErrorBoundary } from '../';
import { ApiError } from '../../autoapi/base';
import { WindowTemplate } from './WindowTemplate';
import { StringUtils } from '../../utils';


interface WindowProps
{
    title: string;
    children: (JSX.Element | JSX.Element[]);
    onOk?: () => void;
    onCancel?: () => void;
    isEnabled?: boolean;
    isOkEnabled?: boolean;
    error?: ApiError | null | undefined;
    level?: number;
}

export function Window(props: WindowProps)
{
    const header =
        <>
            <h5 className={styles.modal_title}>{StringUtils.Truncate40(props.title)}</h5>
            {props.onCancel !== undefined &&
                <button type="button" className="close" aria-label="Close" onClick={props.onCancel}>
                    <span aria-hidden="true">&times;</span>
                </button>
            }
        </>
        ;
    const okButtonStyle = "btn btn-primary" + (props.isOkEnabled ? "" : " disabled"); 
    const footer = props.onOk !== undefined ? <button type="button" className={okButtonStyle} onClick={props.isOkEnabled ? props.onOk : undefined}>Ok</button> : null;


    return (
        <WindowTemplate footer={footer} header={header} isEnabled={props.isEnabled!} level={props.level!}>
            <ErrorBoundary error={props.error}>
                {props.children}
            </ErrorBoundary>
        </WindowTemplate>
        );
}

Window.defaultProps =
{
    level: 0,
    isEnabled: true,
    isOkEnabled : true
}

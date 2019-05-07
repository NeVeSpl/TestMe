import * as React from 'react';
import styles from './Window.module.css';

interface WindowTemplateProps
{
    header: JSX.Element;
    children: JSX.Element;
    footer: JSX.Element | null;
    isEnabled: boolean;
    level: number;
}

export function WindowTemplate(props: WindowTemplateProps)
{
    const style = styles.modal + " " + styles["lvl" + props.level];
    return (
        <div className={style}>
            <div className={styles.modal_dialog}>
                <div className={styles.modal_content + " " + (!props.isEnabled && styles.modal_content_disabled || "")}>
                    <div className={styles.modal_header}>
                        {props.header}
                    </div>
                    <div className={styles.modal_body}>
                        {props.children}
                    </div>
                    <div className={styles.modal_footer}>     
                        {props.footer}
                    </div>
                    {!props.isEnabled && <div className={styles.modal_disabled}></div>}
                </div>                
            </div>
        </div>
        );
}

WindowTemplate.defaultProps =
{
    isEnabled: true,
}

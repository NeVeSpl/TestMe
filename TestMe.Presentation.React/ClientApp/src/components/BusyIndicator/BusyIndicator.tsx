import * as React from 'react';
import styles from './BusyIndicator.module.css';

interface BusyIndicatorProp
{
    isBusy: boolean;
    children: ()=> any;
}


export class BusyIndicator extends React.Component<BusyIndicatorProp>
{
    isFirstTimeShowed: boolean = true;

    render()
    {
        const showChildren = !this.isFirstTimeShowed || !this.props.isBusy;
        this.isFirstTimeShowed = false;
        const containerStyle = this.props.isBusy ? styles.container_busy : styles.container;

        return (
            <div className={containerStyle}>
                {showChildren && this.props.children()}
                {this.props.isBusy &&
                    <div className={styles.modal}>                       
                        <div className="spinner-grow" role="status">
                            <span className="sr-only">Loading...</span>
                        </div>  
                    </div>
                }
            </div>            
        );
    }
}

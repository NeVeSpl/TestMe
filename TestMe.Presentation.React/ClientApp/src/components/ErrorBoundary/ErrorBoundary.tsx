import * as React from 'react';
import { ApiError, ErrorCode }  from '../../api';

interface ErrorBoundaryProp
{
    error: ApiError | null | undefined;
    children: (JSX.Element | JSX.Element[]);
}
class ErrorBoundaryState
{
    hasErrorDuringRender: boolean;
    error: ApiError | null;
    isClosed: boolean;

    constructor()
    {
        this.hasErrorDuringRender = false;
        this.error = null;
        this.isClosed = true;
    }
}

export class ErrorBoundary extends React.Component<ErrorBoundaryProp, ErrorBoundaryState>
{
    state = new ErrorBoundaryState();


    static getDerivedStateFromError(error: any): ErrorBoundaryState
    {        
        return { hasErrorDuringRender: true } as ErrorBoundaryState;
    }
    static getDerivedStateFromProps(props: ErrorBoundaryProp, state: ErrorBoundaryState): ErrorBoundaryState | null
    {
        if ((props.error != undefined) && (props.error !== state.error))
        {
            return { error: props.error, isClosed : false } as ErrorBoundaryState;
        }
        return null;
    }


    handleCloseError = () =>
    {
        this.setState({ isClosed: true });       
    }


    render()
    {
        let alertHeading: string | undefined; 
        let alertMessage: string | undefined;
        let alertFooter: string | undefined;

        if (this.state.error !== null) 
        {
            alertHeading = ErrorCode[this.state.error.errorCode];
            alertMessage = this.state.error.detail;
            alertFooter = this.state.error.traceId;
        }
        if (this.state.hasErrorDuringRender)
        {
            alertHeading = "Render error";
            alertMessage = ":(";
            alertFooter = undefined;
        }

        if ((!this.state.isClosed) || (this.state.hasErrorDuringRender))
        {
            return (
                <>
                    {!this.state.hasErrorDuringRender && this.props.children}
                    <div className="alert alert-danger" role="alert">
                        {alertHeading && <h4 className="alert-heading">{alertHeading}</h4>}
                        {alertMessage && <p>{alertMessage}</p>}
                        {alertFooter &&
                            <>
                                <hr/>
                                <p className="mb-0">{alertFooter}</p>
                            </>
                        }
                        
                        {/*!this.state.hasErrorDuringRender &&
                            <button type="button" className="close" onClick={this.handleCloseError}>
                                <span aria-hidden="true">&times;</span>
                            </button>
                        */}
                    </div>
                </>
            );
        }

        return this.props.children;
    }
}

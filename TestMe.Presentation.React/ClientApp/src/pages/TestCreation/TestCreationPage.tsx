import * as React from 'react';
import { RouteComponentProps, Route } from 'react-router-dom';
import { QuestionsCatalogs } from './QuestionsCatalogs';
import { QuestionsCatalogs as QuestionsCatalogsRedux } from './QuestionsCatalogs/QuestionsCatalogs.redux';


interface TestCreationPageProps extends RouteComponentProps
{

}
class TestCreationPageState
{

}


export class TestCreationPage extends React.Component<TestCreationPageProps, TestCreationPageState>
{
    state = new TestCreationPageState();


    render()
    {
        return (
            <div className="container">
                <div className="row">
                    <div className="col-md">
                        <h3>Local state management</h3>
                        <div className="position-relative">
                            <QuestionsCatalogs />
                        </div>
                    </div>
                    <div className="col-md">
                        <h3>Redux state management</h3>
                        <div className="position-relative">
                            <QuestionsCatalogsRedux />
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}
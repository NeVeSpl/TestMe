import * as React  from 'react';
import { RouteComponentProps, Route} from 'react-router-dom';
import { QuestionsCatalogs } from './QuestionsCatalogs';
import { TestsCatalogs  } from './Test';


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
                    <div className="col-md position-relative">
                        <Route component={QuestionsCatalogs} />  
                    </div>                   
                    <div className="col-md position-relative">
                        <Route  component={TestsCatalogs} />                           
                    </div>
                </div>
            </div>
        );
    }
}
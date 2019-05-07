import * as React from 'react';
import { Route, RouteProps, Redirect } from 'react-router';
import { UserService } from '../../services';

interface AuthenticatedRouteProps extends RouteProps
{
    fallbackPath: string;
}

export function AuthenticatedRoute(props: AuthenticatedRouteProps)
{
    const isAuthenticated = UserService.isAuthenticated();    
    return isAuthenticated ? <Route {...props} /> : <Redirect to={props.fallbackPath} />;
}
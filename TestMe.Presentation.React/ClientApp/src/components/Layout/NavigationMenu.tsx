import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import style from './NavigationMenu.module.css';


export function NavigationMenu()
{   
    return (
        <header className={style.boxshadow}>
            <nav className="navbar navbar-light ng-white border-bottom box-shadow mb-3">
                <div className="container">
                    <Link className="navbar-brand" to="/">TestMe</Link>  
                    <ul className="navbar-nav">
                        <li className="nav-item">
                            <Link className="nav-link text-dark" to="/testCreation">Test editor</Link>
                        </li>
                    </ul>
                </div>
            </nav>
        </header>
    );    
}

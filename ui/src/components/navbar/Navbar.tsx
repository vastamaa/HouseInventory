import React from 'react'

// Our stuff
import './Navbar.css';
import Logos from './logos/Logos';
import Links from './links/Links';


// Source: https://www.sitepoint.com/creating-a-navbar-in-react/
const Navbar = (): JSX.Element => {
    return (
        <nav className="navbar" role='navigation'>
            <Logos />
            <Links />
        </nav>
    )
}

export default Navbar
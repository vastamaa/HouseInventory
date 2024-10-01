import React from 'react'
import Logo from './logo/Logo';
import './Logos.css';

const Logos = (): JSX.Element => {
    return (
        <div className="navbar-left">
            <Logo />
        </div>
    )
}

export default Logos
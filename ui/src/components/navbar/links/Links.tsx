import React from 'react'
import Link, { ILinkProps } from './link/Link'

import './Links.css';

const Links = (): JSX.Element => {
    const linkProps: ILinkProps[] = [
        {
            text: 'Login',
            href: '/login'
        },
        {
            text: 'Register',
            href: '/register'
        }
    ]

    return (
        <div className="navbar-right">
            <ul className="nav-links">
                {linkProps.map(linkProp => <Link {...linkProp} />)}
            </ul>
        </div>
    )
}

export default Links
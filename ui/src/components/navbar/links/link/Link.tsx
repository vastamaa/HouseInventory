import React from 'react'

export interface ILinkProps {
    href: string;
    text: string;
}

const Link = (props: ILinkProps): JSX.Element => {
    const { href, text } = props;

    return (
        <>
            <li>
                <a href={href}>{text}</a>
            </li>
        </>
    )
}

export default Link
import React from 'react'

import './Title.css'

interface ITitleProps {
    title: string;
}

const Title = (props: ITitleProps): JSX.Element => {
    const { title } = props;

    return (
        <div className='title-container'>
            <div>{title}</div>
        </div>
    )
}

export default Title
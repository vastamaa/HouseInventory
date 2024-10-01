import React, { MouseEvent } from 'react'

// Our stuff
import './Button.css';

export interface IButtonProps {
    value: string;
    onHandleButtonClick: (event: MouseEvent<HTMLInputElement>) => void;
}

const Button = (props: IButtonProps): JSX.Element => {
    const { value, onHandleButtonClick } = props;

    return (
        <div className='input-container'>
            <input className='input-button' type='submit' onClick={onHandleButtonClick} value={value} />
        </div>
    )
}

export default Button;
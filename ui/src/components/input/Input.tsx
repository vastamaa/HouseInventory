import React, { ChangeEvent } from 'react'

import './Input.css'

interface IInputProps {
    defaultText: string;
    onHandleInputChange: (event: ChangeEvent<HTMLInputElement>) => void;
    value: string;
    type: string;
}

const Input = (props: IInputProps): JSX.Element => {
    const { defaultText, onHandleInputChange, value, type = 'text' } = props;

    return (
        <div className='input-container'>
            <input
                name='emailAddress'
                value={value}
                placeholder={defaultText}
                onChange={onHandleInputChange}
                className='input-box'
                type={type}
            />
        </div>
    )
}

export default Input;
import React, { ChangeEvent } from 'react'

// Our stuff
import './Input.css'

interface IInputProps {
    defaultText: string;
    onHandleInputChange: (event: ChangeEvent<HTMLInputElement>) => void;
    value: string;
    type: string;
    name: string;
}

const Input = (props: IInputProps): JSX.Element => {
    const { defaultText, onHandleInputChange, value, name, type = 'text' } = props;

    return (
        <div className='input-container'>
            <input
                name={name}
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
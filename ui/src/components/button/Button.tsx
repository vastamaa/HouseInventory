import React, { MouseEvent } from 'react'

interface IButtonProps {
    onHandleButtonClick: (event: MouseEvent<HTMLInputElement>) => void;
}

const Button = (props: IButtonProps): JSX.Element => {
    const { onHandleButtonClick } = props;

    return (
        <div className='input-container'>
            <input className='input-button' type="button" onClick={onHandleButtonClick} value={'Log in'} />
        </div>
    )
}

export default Button;
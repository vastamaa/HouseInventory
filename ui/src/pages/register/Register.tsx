import React, { ChangeEvent, MouseEvent, useState } from 'react'

// Our stuff
import { useHttpRequestService } from '../../contexts/HttpRequestServiceContext';
import UriBuilder from '../../utils/UriBuilder';
import Title from '../../components/title/Title';
import Input from '../../components/input/Input';
import Button from '../../components/button/Button';

interface IUserRegister {
    firstName?: string | null;
    lastName?: string | null;
    phoneNumber?: string | null;
    userName: string;
    email: string;
    password: string;
}

const Register = (): JSX.Element => {
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [userRegister, setUserRegister] = useState<IUserRegister>({ userName: '', email: '', password: '' });
    const httpRequestService = useHttpRequestService();

    const handleRegister = async (event: MouseEvent<HTMLInputElement>) => {
        event.preventDefault();
        setIsLoading(true);

        const registerUri = UriBuilder.use().Register().Build();
        try {
            const response = await httpRequestService.PostResourceAsync<IUserRegister>(registerUri, userRegister);
            console.log('Response value:', response);
        }
        catch (error) {
            console.log('Error fetching data:', error);
        }
        finally {
            setIsLoading(false);
        }
    }

    const handleInputChange = (event: ChangeEvent<HTMLInputElement>): void => {
        const { name, value } = event.target;

        setUserRegister((prevState) => ({
            ...prevState,
            [name]: value
        }));
    };

    const content =
        (
            <>
                <Title title={'Register'} />
                <br />
                <InputForm configurations={getRegisterConfig({ onChange: handleInputChange, onClick: handleRegister, formDetails: userRegister })} />
            </>
        )

    return (
        <div className='main-container'>
<<<<<<< HEAD
    { isLoading ? <Loader /> : content }
=======
            <Title title={'Register'} />
            <br />
            <Input onHandleInputChange={handleInputChange} defaultText='Enter your first name here' value={userRegister.firstName} name='firstName' type={'text'} />
            <br />
            <Input onHandleInputChange={handleInputChange} defaultText='Enter your last name here' value={userRegister.lastName} name='lastName' type={'text'} />
            <br />
            <Input onHandleInputChange={handleInputChange} defaultText='Enter your phone number here' value={userRegister.phoneNumber} name='phoneNumber' type={'text'} />
            <br />
            <Input onHandleInputChange={handleInputChange} defaultText='Enter your username here' value={userRegister.userName} name='userName' type={'text'} />
            <br />
            <Input onHandleInputChange={handleInputChange} defaultText='Enter your e-mail here' value={userRegister.email} name='email' type={'email'} />
            <br />
            <Input onHandleInputChange={handleInputChange} defaultText='Enter your password here' value={userRegister.password} name='password' type='password' />
            <br />
            <Button onHandleButtonClick={handleRegister} />
>>>>>>> parent of d48865f (Implemented an easier way to create form inputs. Does not apply the Open/Close principle yet.)
        </div >
    )
}

export default Register
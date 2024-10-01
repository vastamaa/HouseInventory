import React from 'react'
import { PulseLoader } from 'react-spinners'

/* Source: https://www.davidhu.io/react-spinners/ */
const Loader = (): JSX.Element => {
    return (
        <>
            <PulseLoader color='#F5F5F5' />
            <h3>Loading...</h3>
        </>
    )
}

export default Loader
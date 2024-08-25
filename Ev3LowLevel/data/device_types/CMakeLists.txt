
set (RCF_FILES
    typedata.rcf
    typedata50.rcf
)

foreach (rcf ${RCF_FILES})
    configure_file(${rcf} ${rcf} COPYONLY)
endforeach ()

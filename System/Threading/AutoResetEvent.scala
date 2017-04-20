package System.Threading;

class AutoResetEvent(b:Boolean)
{

	final val _monitor = new Object();
    @volatile final var _isOpen = b;

    def WaitOne()
    {
      _monitor.synchronized({
            while (!_isOpen) {
                _monitor.wait();
            }
            _isOpen = false;
        
      });
    }

    def WaitOne(timeout:Long)
    {
        _monitor.synchronized({
            val t = java.lang.System.currentTimeMillis();
            while (!_isOpen) {
                _monitor.wait(timeout);
                // Check for timeout
                if (java.lang.System.currentTimeMillis() - t >= timeout)
                {
                  _isOpen = false;
                  return;
                }
            }
            
        });
    }

    def Set()
    {
        _monitor.synchronized({
            _isOpen = true;
            _monitor.notify();
        });
    }

    def Reset()
    {
        _isOpen = false;
    }
	
}